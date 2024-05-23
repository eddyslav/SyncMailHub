using Google;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;

using Google.Apis.Services;

using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System.Collections.Concurrent;

namespace Modules.Sync.Infrastructure.Emails.Google;

internal class ServiceAccountSyncSession(GoogleServiceAccountCredentials credentials
	, IServiceAccountContextAccessor accountContextAccessor
	, IServiceAccountSyncStateRepository syncStateRepository
	, IUnitOfWork unitOfWork
	, ILogger logger
	, IOptions<ServiceAccountSyncSessionConfiguration> options) : IServiceAccountSyncSession
{
	private static readonly IReadOnlyCollection<string> scopes =
	[
		GmailService.Scope.GmailReadonly,
	];

	private static readonly string currentUserId = "me";

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

	private readonly GmailService service = CreateService(credentials);
	private readonly ILogger logger = logger.ForContext<ServiceAccountSyncSession>();
	private readonly ServiceAccountSyncSessionConfiguration configuration = options.GetConfiguration();

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

	private Task<Result<ulong>> GetLastHistoryAsync(CancellationToken cancellationToken) =>
		Result.Create(service.Users.GetProfile(currentUserId))
			.Bind(profileRequest =>
				TryExecuteRequestAsync<UsersResource.GetProfileRequest, Profile>(profileRequest, cancellationToken))
			.Bind(profileResponse => Result.Create(profileResponse.HistoryId));

	public async IAsyncEnumerable<Result<IReadOnlyList<IServiceAccountStateChange>>> ProcessStateChangesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
	{
		var account = accountContextAccessor.Account;
		var lastSyncState = (GoogleServiceAccountSyncState?)await syncStateRepository.GetByAccountIdAsync(account.Id, cancellationToken);

		if (lastSyncState is null)
		{
			var lastHistoryResult = await GetLastHistoryAsync(cancellationToken);
			if (lastHistoryResult.IsSuccess)
			{
				lastSyncState = GoogleServiceAccountSyncState.Create(account.Id, lastHistoryResult.Value);
				syncStateRepository.Add(lastSyncState);
				await unitOfWork.SaveChangesAsync(cancellationToken);
			}

			yield break;
		}

		var request = service.Users.History.List(currentUserId);
		request.MaxResults = configuration.MaxHistoriesToRetrievePerRequest;
		request.StartHistoryId = lastSyncState.HistoryId;

		var maxHistoryId = lastSyncState.HistoryId;

		do
		{
			var changesResponseResult = await TryExecuteRequestAsync<UsersResource.HistoryResource.ListRequest, ListHistoryResponse>(request, cancellationToken);

			if (!changesResponseResult.IsSuccess)
			{
				yield return Result.Failure<IReadOnlyList<IServiceAccountStateChange>>(changesResponseResult.Error);
				break;
			}

			var changesResponse = changesResponseResult.Value;
			var changes = changesResponse.History;
			if (!(changes?.Any() ?? false))
			{
				if (changesResponse.HistoryId.HasValue)
				{
					lastSyncState.UpdateSyncState(changesResponse.HistoryId.Value);
					await unitOfWork.SaveChangesAsync(cancellationToken);
				}

				yield return Array.Empty<IServiceAccountStateChange>();
				break;
			}

			var folderIdsByConversationIds = new ConcurrentDictionary<string, HashSet<string>>();

			void AddConversationChange(Message sourceMessage)
			{
				folderIdsByConversationIds.AddOrUpdate(sourceMessage.ThreadId
					, _ => [..sourceMessage.LabelIds ?? []]
					, (_, folderIds) =>
					{
						folderIds.UnionWith(sourceMessage.LabelIds);
						return folderIds;
					});
			}

			foreach (var change in changes)
			{
				maxHistoryId = change.Id.HasValue
					? maxHistoryId > change.Id.Value
						? maxHistoryId
						: change.Id.Value
					: maxHistoryId;

				var messagesDeleted = change.MessagesDeleted ?? [];
				messagesDeleted.ForEach(messageDeleted => AddConversationChange(messageDeleted.Message));

				var labelsAdded = change.LabelsAdded ?? [];
				labelsAdded.ForEach(labelAdded => AddConversationChange(labelAdded.Message));

				var labelsRemoved = change.LabelsRemoved ?? [];
				labelsRemoved.ForEach(labelRemoved => AddConversationChange(labelRemoved.Message));

				var messagesAdded = change.MessagesAdded ?? [];
				messagesAdded.ForEach(messageAdded => AddConversationChange(messageAdded.Message));
			}

			yield return folderIdsByConversationIds
				.Select(entry => new EmailsConversationArtifactStateChange(entry.Key, entry.Value.ToList()))
				.ToList();

			request.PageToken = changesResponse.NextPageToken;

			lastSyncState.UpdateSyncState(maxHistoryId);
			await unitOfWork.SaveChangesAsync(cancellationToken);
		} while (!string.IsNullOrEmpty(request.PageToken));
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);

		service.Dispose();
	}
}
