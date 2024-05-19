using System.Net;

using System.Text;

using Microsoft.IdentityModel.Tokens;

using MimeKit;

using Google;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;

using Google.Apis.Requests;
using Google.Apis.Services;

using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using EmailsThread = Google.Apis.Gmail.v1.Data.Thread;
using MessagePart = Google.Apis.Gmail.v1.Data.MessagePart;

namespace Modules.Hub.Infrastucture.Emails.MailService.Google;

internal sealed class MailService(GoogleServiceAccountCredentials credentials
	, ILogger logger
	, IOptions<MailServiceConfiguration> options) : IMailService
{
	private static class HeaderNames
	{
		public const string Subject = "Subject";
		public const string FromRecipient = "From";
		public const string ToRecipients = "To";
		public const string CcRecipients = "Cc";
		public const string BccRecipients = "Bcc";
	}

	private static readonly IReadOnlyCollection<string> scopes =
	[
		GmailService.Scope.MailGoogleCom,
	];

	private static readonly ICollection<string> defaultFoldersToIgnoreEmailIn = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"SPAM",
		"TRASH"
	};

	private const string currentUserId = "me";

	private readonly GmailService service = CreateService(credentials);
	private readonly ILogger logger = logger.ForContext<MailService>();
	private readonly MailServiceConfiguration configuration = options.GetConfiguration();

	private static GmailService CreateService(GoogleServiceAccountCredentials credentials)
	{
		var initializer = new GoogleAuthorizationCodeFlow.Initializer
		{
			ClientSecrets = new ClientSecrets
			{
				ClientId = credentials.ClientId,
				ClientSecret = credentials.ClientSecret,
			},
			Scopes = scopes,
		};

		var tokenResponse = new TokenResponse
		{
			RefreshToken = credentials.RefreshToken,
		};

		var googleCredentials = new UserCredential(new GoogleAuthorizationCodeFlow(initializer), currentUserId, tokenResponse);

		return new GmailService(new BaseClientService.Initializer
		{
			HttpClientInitializer = googleCredentials,
		});
	}

	private async Task<Result<TResponse>> TryExecuteRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
		where TRequest : GmailBaseServiceRequest<TResponse>
		where TResponse : notnull
	{
		try
		{
			return await request.ExecuteAsync(cancellationToken);
		}
		catch (GoogleApiException apiException)
		{
			logger.Error(apiException, "Failed to retrieve results");
			return Result.Failure<TResponse>(ServiceAccountErrors.Google.ApiRequestFailed);
		}
	}

	private async Task<Result<IReadOnlyList<T>>> TryGetFullResourcesWithRetriesAsync<T>(ICollection<T> sourceEntities
		, Func<string, T, GmailService, IClientServiceRequest> requestFactory
		, CancellationToken cancellationToken)
		where T : class
	{
		var fullEntities = new List<T>(sourceEntities.Count);
		var entitiesToRetry = default(ICollection<T>);

		Task ProcessBatchAsync(IEnumerable<T> entities)
		{
			var batchRequest = new BatchRequest(service);

			foreach (var entity in entities)
			{
				var entityRequest = requestFactory(currentUserId, entity, service);

				// retries are necessary as quota is easily hit
				batchRequest.Queue<T>(entityRequest
					, (fullEntity, error, _, _) =>
					{
						if (error is null)
						{
							fullEntities.Add(fullEntity);
							entitiesToRetry?.Remove(entity);

							return;
						}

						logger.Error("Batch request was failed for email: {errorMessage}", error.Message);

						entitiesToRetry ??= new List<T>();
						entitiesToRetry.Add(entity);
					});
			}

			return batchRequest.ExecuteAsync(cancellationToken);
		}

		await ProcessBatchAsync(sourceEntities);

		var retriesLeft = configuration.MaxRetriesToLoadEntities;
		while ((entitiesToRetry?.Any() ?? false) && retriesLeft > 0)
		{
			retriesLeft--;

			await ProcessBatchAsync(entitiesToRetry);
		}

		if (entitiesToRetry?.Any() ?? false)
		{
			return Result.Failure<IReadOnlyList<T>>(ServiceAccountErrors.Google.ApiRequestFailed);
		}

		return fullEntities;
	}

	private async Task<Result<IReadOnlyList<T>>> TryGetFullResourcesInChunksAsync<T>(ICollection<T> sourceEntities
		, Func<string, T, GmailService, IClientServiceRequest> requestFactory
		, CancellationToken cancellationToken)
		where T : class
	{
		var results = new List<T>(sourceEntities.Count);
		var sourceEntitiesChunks = sourceEntities.Chunk(configuration.MaxEntitiesPerBatch);

		foreach (var sourceMessagesChunk in sourceEntitiesChunks)
		{
			var chunkResultsResult = await TryGetFullResourcesWithRetriesAsync(sourceMessagesChunk, requestFactory, cancellationToken);
			if (!chunkResultsResult.IsSuccess)
			{
				return chunkResultsResult;
			}

			results.AddRange(chunkResultsResult.Value);
		}

		return results;
	}

	private Task<Result<IReadOnlyList<T>>> TryGetFullResourcesAsync<T>(ICollection<T> sourceEntities
		, Func<string, T, GmailService, IClientServiceRequest> requestFactory
		, CancellationToken cancellationToken)
		where T : class =>
		sourceEntities.Count > configuration.MaxEntitiesPerBatch
			? TryGetFullResourcesInChunksAsync(sourceEntities, requestFactory, cancellationToken)
			: TryGetFullResourcesWithRetriesAsync(sourceEntities, requestFactory, cancellationToken);

	public Task<Result<EmailsCounter>> GetEmailsCountAsync(CancellationToken cancellationToken) =>
		Result.Create(service.Users.GetProfile(currentUserId))
			.Bind(profileRequest => TryExecuteRequestAsync<UsersResource.GetProfileRequest, Profile>(profileRequest, cancellationToken))
			.Map(profile => new EmailsCounter(profile.MessagesTotal ?? 0));

	public Task<Result<IReadOnlyList<EmailsFolder>>> GetEmailsFoldersAsync(CancellationToken cancellationToken = default)
	{
		static IEnumerable<EmailsFolder> MapToModel(ICollection<Label> labels)
		{
			var labelNamesToId = default(IReadOnlyDictionary<string, string>);

			const string folderHiddenVisibilityOption = "labelHide";

			foreach (var label in labels)
			{
				if (label.LabelListVisibility == folderHiddenVisibilityOption)
				{
					continue;
				}

				var labelName = label.Name;

				var labelNameSpan = labelName.AsSpan();
				var slashIndex = labelNameSpan.IndexOf('/');

				var parentFolderName = slashIndex <= 0
					? null
					: labelNameSpan[..slashIndex].ToString();

				if (parentFolderName is not null)
				{
					labelNamesToId ??= labels.ToDictionary(label => label.Name, label => label.Id);
				}

				var parentLabelId = default(string);
				if (parentFolderName is not null)
				{
					labelNamesToId!.TryGetValue(parentFolderName, out parentLabelId);
				}

				labelName = parentLabelId is null
					? labelName
					: slashIndex == labelNameSpan.Length
						? "/"
						: labelNameSpan[(slashIndex + 1)..].ToString();

				yield return new EmailsFolder(label.Id
					, labelName
					, parentLabelId);
			}
		}

		return Result.Create(service.Users.Labels.List(currentUserId))
			.Bind(labelsRequest => TryExecuteRequestAsync<UsersResource.LabelsResource.ListRequest, ListLabelsResponse>(labelsRequest, cancellationToken))
			.Map(labelsResponse => MapToModel(labelsResponse.Labels).ToList() as IReadOnlyList<EmailsFolder>);
	}

	public Task<Result<EmailsFolderCount>> GetEmailsFolderCountAsync(string folderId, CancellationToken cancellationToken = default)
	{
		async Task<Result<Label>> GetTargetLabelAsync(string folderId, CancellationToken cancellationToken)
		{
			var request = service.Users.Labels.Get(currentUserId, folderId);

			try
			{
				return await request.ExecuteAsync(cancellationToken);
			}
			catch (GoogleApiException apiException) when (apiException.HttpStatusCode == HttpStatusCode.NotFound)
			{
				return Result.Failure<Label>(EmailsFolderErrors.NotFound);
			}
			catch (GoogleApiException apiException) when (apiException.HttpStatusCode == HttpStatusCode.NotFound)
			{
				logger.Error(apiException, "Failed to retrieve statistics for \"{folderId}\" folder", folderId);
				return Result.Failure<Label>(ServiceAccountErrors.Google.ApiRequestFailed);
			}
		}

		return Result.Success(folderId)
			.Bind(folderId => GetTargetLabelAsync(folderId, cancellationToken))
			.Map(label => new EmailsFolderCount(label.MessagesTotal!.Value, label.MessagesUnread!.Value));
	}

	public Task<Result<PaginatedList<EmailsConversation>>> GetEmailsConversationsAsync(string folderId
		, string? nextPageToken
		, CancellationToken cancellationToken = default)
	{
		static IClientServiceRequest CreateRequest(string currentUserId
			, EmailsThread thread
			, GmailService service)
		{
			var request = service.Users.Threads.Get(currentUserId, thread.Id);
			request.Format = UsersResource.ThreadsResource.GetRequest.FormatEnum.Metadata;

			return request;
		}

		static IEnumerable<EmailsConversation> MapToModel(IEnumerable<EmailsThread> sourceThreads)
		{
			foreach (var thread in sourceThreads)
			{
				var firstEmail = thread.Messages.MinBy(threadMessage => threadMessage.InternalDate);
				Debug.Assert(firstEmail is not null);

				// for drafts
				var subject = firstEmail.Payload.Headers
					.FirstOrDefault(header => header.Name.Equals(HeaderNames.Subject, StringComparison.OrdinalIgnoreCase));

				yield return new EmailsConversation(thread.Id, subject?.Value);
			}
		}

		async Task<Result<ListThreadsResponse>> GetThreadsInFolderAsync(string folderId
			, string? pageToken
			, CancellationToken cancellationToken)
		{
			var request = service.Users.Threads.List(currentUserId);
			request.LabelIds = folderId;
			request.PageToken = nextPageToken;

			try
			{
				return await request.ExecuteAsync(cancellationToken);
			}
			catch (GoogleApiException apiException)
				when (apiException.HttpStatusCode == HttpStatusCode.BadRequest
					&& apiException.Message.Contains("label"))
			{
				return Result.Failure<ListThreadsResponse>(EmailsFolderErrors.NotFound);
			}
			catch (GoogleApiException apiException) when (apiException.HttpStatusCode == HttpStatusCode.NotFound)
			{
				logger.Error(apiException, "Failed to retrieve threads per \"{folderId}\" folder", folderId);
				return Result.Failure<ListThreadsResponse>(ServiceAccountErrors.Google.ApiRequestFailed);
			}
		}

		Task<Result<PaginatedList<EmailsConversation>>> GetEmailsConversationsAsync(ListThreadsResponse sourceResponse, CancellationToken cancellationToken) =>
			Result.Create(sourceResponse)
				.Bind(sourceResponse => TryGetFullResourcesAsync<EmailsThread>(sourceResponse.Threads, CreateRequest, cancellationToken))
				.Map(fullThreads => MapToModel(fullThreads).ToList())
				.Map(emailsConversations => new PaginatedList<EmailsConversation>(emailsConversations, sourceResponse.NextPageToken));

		return Result.Success()
			.Bind(() => GetThreadsInFolderAsync(folderId, nextPageToken, cancellationToken))
			.Bind(fullThreadsResponse => GetEmailsConversationsAsync(fullThreadsResponse, cancellationToken));
	}

	public Task<Result<IReadOnlyList<Email>>> GetEmailsAsync(string conversationId, CancellationToken cancellationToken = default)
	{
		static string? TryGetHtmlBody(MessagePart payload)
		{
			const string htmlBodyPartMimeType = "text/html";

			if (payload.MimeType.Equals(htmlBodyPartMimeType, StringComparison.OrdinalIgnoreCase))
			{
				var htmlBodyPartBody = payload.Body;

				if (htmlBodyPartBody.Size == 0)
				{
					return null;
				}

				var rawContent = Base64UrlEncoder.DecodeBytes(htmlBodyPartBody.Data);
				return Encoding.UTF8.GetString(rawContent);
			}

			var payloadParts = payload.Parts ?? [];
			if (!payloadParts.Any())
			{
				return null;
			}

			foreach (var payloadPart in payloadParts)
			{
				if (TryGetHtmlBody(payloadPart) is string htmlBody)
				{
					return htmlBody;
				}
			}

			return null;
		}

		static Email.EmailRecipient MapToRecipientModel(MailboxAddress mailboxAddress
			, Email.EmailRecipient.EmailRecipientType sourceType) =>
			new(mailboxAddress.Name, mailboxAddress.Address, sourceType);

		static IEnumerable<Email> MapToModel(IEnumerable<Message> sourceMessages)
		{
			static IEnumerable<Email.EmailRecipient> TryParseRecipients(IEnumerable<MessagePartHeader> sourceHeaders
				, string recipientsHeaderName
				, Email.EmailRecipient.EmailRecipientType sourceType)
			{
				var rawRecipients = sourceHeaders.FirstOrDefault(header =>
					header.Name.Equals(recipientsHeaderName, StringComparison.OrdinalIgnoreCase))?.Value;

				if (string.IsNullOrEmpty(rawRecipients))
				{
					yield break;
				}

				var parsedEmailAddresses = InternetAddressList.Parse(rawRecipients);
				foreach (var parsedEmailAddress in parsedEmailAddresses.Mailboxes)
				{
					yield return MapToRecipientModel(parsedEmailAddress, sourceType);
				}
			}

			foreach (var message in sourceMessages)
			{
				if (message.LabelIds.Any(defaultFoldersToIgnoreEmailIn.Contains))
				{
					continue;
				}

				var messagePayload = message.Payload;
				var headers = messagePayload.Headers;
				Debug.Assert(headers?.Any() ?? false);

				var rawFromEmailAddress = headers.First(header =>
					header.Name.Equals(HeaderNames.FromRecipient, StringComparison.OrdinalIgnoreCase)).Value;
				var parsedFromEmailAddress = MailboxAddress.Parse(rawFromEmailAddress);

				var recipients = Enumerable.Empty<Email.EmailRecipient>();
				recipients = recipients.Append(MapToRecipientModel(parsedFromEmailAddress, Email.EmailRecipient.EmailRecipientType.From));
				recipients = recipients.Concat(TryParseRecipients(headers, HeaderNames.ToRecipients, Email.EmailRecipient.EmailRecipientType.To));
				recipients = recipients.Concat(TryParseRecipients(headers, HeaderNames.CcRecipients, Email.EmailRecipient.EmailRecipientType.Cc));
				recipients = recipients.Concat(TryParseRecipients(headers, HeaderNames.BccRecipients, Email.EmailRecipient.EmailRecipientType.Bcc));

				var htmlBody = TryGetHtmlBody(messagePayload);
				var emailAddressesList = recipients.ToList();

				yield return new Email(message.Id
					, htmlBody
					, emailAddressesList);
			}
		}

		async Task<Result<EmailsThread>> GetTargetThreadAsync(string conversationId, CancellationToken cancellationToken)
		{
			var request = service.Users.Threads.Get(currentUserId, conversationId);
			request.Format = UsersResource.ThreadsResource.GetRequest.FormatEnum.Full;

			try
			{
				return await request.ExecuteAsync(cancellationToken);
			}
			catch (GoogleApiException apiException) when (apiException.HttpStatusCode == HttpStatusCode.NotFound)
			{
				return Result.Failure<EmailsThread>(EmailsConversationErrors.NotFound);
			}
			catch (GoogleApiException apiException) when (apiException.HttpStatusCode == HttpStatusCode.NotFound)
			{
				logger.Error(apiException, "Failed to retrieve \"{conversationId}\" conversation", conversationId);
				return Result.Failure<EmailsThread>(ServiceAccountErrors.Google.ApiRequestFailed);
			}
		}

		return Result.Success(conversationId)
			.Bind(conversationId => GetTargetThreadAsync(conversationId, cancellationToken))
			.Map(emailsThread => MapToModel(emailsThread.Messages).ToList() as IReadOnlyList<Email>);
	}

	public Task<Result<SendEmailResponse>> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken = default)
	{
		static void CopyRecipients(IReadOnlyList<string>? sourceRecipients, InternetAddressList target)
		{
			if (!(sourceRecipients?.Any() ?? false))
			{
				return;
			}

			target.AddRange(sourceRecipients.Select(recipient => new MailboxAddress(null, recipient)));
		}

		static MimeMessage MapToMimeMessage(SendEmailRequest request)
		{
			var mimeMessage = new MimeMessage
			{
				Subject = request.Subject
			};

			var bodyBuilder = new BodyBuilder
			{
				HtmlBody = request.Body
			};
			mimeMessage.Body = bodyBuilder.ToMessageBody();

			CopyRecipients(request.To, mimeMessage.To);
			CopyRecipients(request.Cc, mimeMessage.Cc);
			CopyRecipients(request.Bcc, mimeMessage.Bcc);

			return mimeMessage;
		}

		static async Task<Message> MapToMessageAsync(MimeMessage sourceMessage, CancellationToken cancellationToken)
		{
			var rawMessageContent = default(string);

			using var targetStream = new MemoryStream();
			await sourceMessage.WriteToAsync(targetStream, cancellationToken);

			rawMessageContent = Base64UrlEncoder.Encode(targetStream.ToArray());

			return new Message
			{
				Raw = rawMessageContent,
			};
		}

		return Result.Success(request)
			.Map(MapToMimeMessage)
			.Map(mimeMessage => MapToMessageAsync(mimeMessage, cancellationToken))
			.Map(message => service.Users.Messages.Send(message, currentUserId))
			.Bind(request => TryExecuteRequestAsync<UsersResource.MessagesResource.SendRequest, Message>(request, cancellationToken))
			.Map(response => new SendEmailResponse(response.ThreadId, response.Id));
	}

	public Task<Result<SendReplyResponse>> SendReplyToConversationAsync(string conversationId
		, string body
		, CancellationToken cancellationToken = default)
	{
		static InternetAddressList? TryGetRecipients(Message sourceMessage, string recipientsType)
		{
			var header = sourceMessage.Payload.Headers.FirstOrDefault(header =>
				header.Name.Equals(recipientsType, StringComparison.OrdinalIgnoreCase))?.Value;

			if (string.IsNullOrEmpty(header))
			{
				return null;
			}

			return InternetAddressList.Parse(header);
		}

		static void CopyRecipients(Message sourceMessage
			, string recipientsType
			, InternetAddressList target)
		{
			var sourceRecipients = TryGetRecipients(sourceMessage, recipientsType);
			if (sourceRecipients is null)
			{
				return;
			}

			target.AddRange(sourceRecipients);
		}

		static MimeMessage CreateMimeMessage(string body, Message sourceMessage)
		{
			var headers = sourceMessage.Payload.Headers;
			var subject = headers
				.First(header => header.Name.Equals(HeaderNames.Subject, StringComparison.OrdinalIgnoreCase))
				.Value;

			var mimeMessage = new MimeMessage
			{
				Subject = subject,
			};

			CopyRecipients(sourceMessage, HeaderNames.FromRecipient, mimeMessage.To);
			CopyRecipients(sourceMessage, HeaderNames.ToRecipients, mimeMessage.To);
			CopyRecipients(sourceMessage, HeaderNames.CcRecipients, mimeMessage.Cc);

			var bodyBuilder = new BodyBuilder
			{
				HtmlBody = body,
			};

			mimeMessage.Body = bodyBuilder.ToMessageBody();

			return mimeMessage;
		}

		static async Task<Message> CreateMessageAsync(string conversationId
			, MimeMessage sourceMessage
			, CancellationToken cancellationToken)
		{
			var rawMessageContent = default(string);

			using var targetStream = new MemoryStream();
			await sourceMessage.WriteToAsync(targetStream, cancellationToken);

			rawMessageContent = Base64UrlEncoder.Encode(targetStream.ToArray());

			return new Message
			{
				Raw = rawMessageContent,
				ThreadId = conversationId,
			};
		}

		async Task<Result<EmailsThread>> GetTargetThreadAsync(string conversationId, CancellationToken cancellationToken)
		{
			var threadRequest = service.Users.Threads.Get(currentUserId, conversationId);
			threadRequest.Format = UsersResource.ThreadsResource.GetRequest.FormatEnum.Metadata;
			threadRequest.MetadataHeaders = new[]
			{
				HeaderNames.Subject,
				HeaderNames.FromRecipient,
				HeaderNames.ToRecipients,
				HeaderNames.CcRecipients,
			};

			threadRequest.Fields = "messages";

			try
			{
				return await threadRequest.ExecuteAsync(cancellationToken);
			}
			catch (GoogleApiException apiException) when (apiException.HttpStatusCode == HttpStatusCode.NotFound)
			{
				return Result.Failure<EmailsThread>(EmailsConversationErrors.NotFound);
			}
			catch (GoogleApiException apiException)
			{
				logger.Error(apiException, "Failed to retrieve \"{conversationId}\" thread", conversationId);
				return Result.Failure<EmailsThread>(ServiceAccountErrors.Google.ApiRequestFailed);
			}
		}

		async Task<Result<Message>> TrySendResponseAsync(Message message, CancellationToken cancellationToken)
		{
			var request = service.Users.Messages.Send(message, currentUserId);

			try
			{
				return await request.ExecuteAsync(cancellationToken);
			}
			catch (GoogleApiException apiException) when (apiException.HttpStatusCode == HttpStatusCode.NotFound)
			{
				return Result.Failure<Message>(EmailsConversationErrors.NotFound);
			}
			catch (GoogleApiException apiException)
			{
				logger.Error(apiException, "Failed to send reply to \"{conversationId}\" conversation", conversationId);
				return Result.Failure<Message>(ServiceAccountErrors.Google.ApiRequestFailed);
			}
		}

		return Result.Success()
			.Bind(() => GetTargetThreadAsync(conversationId, cancellationToken))
			.Map(thread => thread.Messages.First())
			.Map(firstMessage => CreateMimeMessage(body, firstMessage))
			.Map(mimeMessage => CreateMessageAsync(conversationId, mimeMessage, cancellationToken))
			.Bind(sourceMessage => TrySendResponseAsync(sourceMessage, cancellationToken))
			.Map(response => new SendReplyResponse(response.Id));
	}

	public Task<Result> TrashEmailByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		async Task<Result> TryExecuteAsync(string sourceId
			, UsersResource.MessagesResource.TrashRequest request
			, CancellationToken cancellationToken)
		{
			try
			{
				await request.ExecuteAsync(cancellationToken);
				return Result.Success();
			}
			catch (GoogleApiException apiException) when (apiException.HttpStatusCode == HttpStatusCode.NotFound)
			{
				return Result.Failure(EmailErrors.NotFound);
			}
			catch (GoogleApiException apiException)
			{
				logger.Error(apiException, "Failed to trash \"{emailId}\" email", sourceId);
				return Result.Failure(ServiceAccountErrors.Google.ApiRequestFailed);
			}
		}

		return Result.Create(service.Users.Messages.Trash(currentUserId, id))
			.Bind(request => TryExecuteAsync(id, request, cancellationToken));
	}

	public Task<Result> DeleteEmailByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		async Task<Result> TryExecuteAsync(string sourceId
			, UsersResource.MessagesResource.DeleteRequest request
			, CancellationToken cancellationToken)
		{
			try
			{
				await request.ExecuteAsync(cancellationToken);
				return Result.Success();
			}
			catch (GoogleApiException apiException) when (apiException.HttpStatusCode == HttpStatusCode.NotFound)
			{
				return Result.Failure(EmailErrors.NotFound);
			}
			catch (GoogleApiException apiException)
			{
				logger.Error(apiException, "Failed to trash \"{emailId}\" email", sourceId);
				return Result.Failure(ServiceAccountErrors.Google.ApiRequestFailed);
			}
		}

		return Result.Create(service.Users.Messages.Delete(currentUserId, id))
			.Bind(request => TryExecuteAsync(id, request, cancellationToken));
	}

	public Task<Result> TrashConversationByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		async Task<Result> TryExecuteAsync(string sourceId
			, UsersResource.ThreadsResource.TrashRequest request
			, CancellationToken cancellationToken)
		{
			try
			{
				await request.ExecuteAsync(cancellationToken);
				return Result.Success();
			}
			catch (GoogleApiException apiException) when (apiException.HttpStatusCode == HttpStatusCode.NotFound)
			{
				return Result.Failure(EmailsConversationErrors.NotFound);
			}
			catch (GoogleApiException apiException)
			{
				logger.Error(apiException, "Failed to trash \"{conversationId}\" conversation", sourceId);
				return Result.Failure(ServiceAccountErrors.Google.ApiRequestFailed);
			}
		}

		return Result.Create(service.Users.Threads.Trash(currentUserId, id))
			.Bind(request => TryExecuteAsync(id, request, cancellationToken));
	}

	public Task<Result> DeleteConversationByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		async Task<Result> TryExecuteAsync(string sourceId
			, UsersResource.ThreadsResource.DeleteRequest request
			, CancellationToken cancellationToken)
		{
			try
			{
				await request.ExecuteAsync(cancellationToken);
				return Result.Success();
			}
			catch (GoogleApiException apiException) when (apiException.HttpStatusCode == HttpStatusCode.NotFound)
			{
				return Result.Failure(EmailsConversationErrors.NotFound);
			}
			catch (GoogleApiException apiException)
			{
				logger.Error(apiException, "Failed to delete \"{conversationId}\" conversation", sourceId);
				return Result.Failure(ServiceAccountErrors.Google.ApiRequestFailed);
			}
		}

		return Result.Create(service.Users.Threads.Delete(currentUserId, id))
			.Bind(request => TryExecuteAsync(id, request, cancellationToken));
	}
}
