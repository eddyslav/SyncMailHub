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
	private static readonly IReadOnlyCollection<string> scopes =
	[
		GmailService.Scope.GmailReadonly,
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

	public Task<Result<EmailsFolderCount>> GetEmailsFolderCountAsync(string folderId, CancellationToken cancellationToken = default) =>
		Result.Create(service.Users.Labels.Get(currentUserId, folderId))
			.Bind(labelsRequest => TryExecuteRequestAsync<UsersResource.LabelsResource.GetRequest, Label>(labelsRequest, cancellationToken))
			.Map(label => new EmailsFolderCount(label.MessagesTotal!.Value, label.MessagesUnread!.Value));

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
			const string subjectHeaderName = "Subject";

			foreach (var thread in sourceThreads)
			{
				var firstEmail = thread.Messages.MinBy(threadMessage => threadMessage.InternalDate);
				Debug.Assert(firstEmail is not null);

				// for drafts
				var subject = firstEmail.Payload.Headers
					.FirstOrDefault(header => header.Name.Equals(subjectHeaderName, StringComparison.OrdinalIgnoreCase));

				yield return new EmailsConversation(thread.Id, subject?.Value);
			}
		}

		Task<Result<PaginatedList<EmailsConversation>>> GetEmailsConversationsAsync(ListThreadsResponse sourceResponse, CancellationToken cancellationToken) =>
			Result.Create(sourceResponse)
				.Bind(sourceResponse => TryGetFullResourcesAsync<EmailsThread>(sourceResponse.Threads, CreateRequest, cancellationToken))
				.Map(fullThreads => MapToModel(fullThreads).ToList())
				.Map(emailsConversations => new PaginatedList<EmailsConversation>(emailsConversations, sourceResponse.NextPageToken));

		var request = service.Users.Threads.List(currentUserId);
		request.LabelIds = folderId;
		request.PageToken = nextPageToken;

		return Result.Success(request)
			.Bind(threadsRequest =>
				TryExecuteRequestAsync<UsersResource.ThreadsResource.ListRequest, ListThreadsResponse>(threadsRequest, cancellationToken))
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

			const string fromAddressHeaderName = "From";
			const string toAddressesHeaderName = "To";
			const string ccAddressesHeaderName = "Cc";
			const string bccAddressesHeaderName = "Bcc";

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
					header.Name.Equals(fromAddressHeaderName, StringComparison.OrdinalIgnoreCase)).Value;
				var parsedFromEmailAddress = MailboxAddress.Parse(rawFromEmailAddress);

				var recipients = Enumerable.Empty<Email.EmailRecipient>();
				recipients = recipients.Append(MapToRecipientModel(parsedFromEmailAddress, Email.EmailRecipient.EmailRecipientType.From));
				recipients = recipients.Concat(TryParseRecipients(headers, toAddressesHeaderName, Email.EmailRecipient.EmailRecipientType.To));
				recipients = recipients.Concat(TryParseRecipients(headers, ccAddressesHeaderName, Email.EmailRecipient.EmailRecipientType.Cc));
				recipients = recipients.Concat(TryParseRecipients(headers, bccAddressesHeaderName, Email.EmailRecipient.EmailRecipientType.Bcc));

				var htmlBody = TryGetHtmlBody(messagePayload);
				var emailAddressesList = recipients.ToList();

				yield return new Email(message.Id
					, htmlBody
					, emailAddressesList);
			}
		}

		var request = service.Users.Threads.Get(currentUserId, conversationId);
		request.Format = UsersResource.ThreadsResource.GetRequest.FormatEnum.Full;

		return Result.Success(request)
			.Bind(request => TryExecuteRequestAsync<UsersResource.ThreadsResource.GetRequest, EmailsThread>(request, cancellationToken))
			.Map(emailsThread => MapToModel(emailsThread.Messages).ToList() as IReadOnlyList<Email>);
	}
}
