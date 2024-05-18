using Modules.Hub.Communication.RegisterServiceAccountStateChange;
using ITargetServiceAccountStateChange = Modules.Hub.Communication.RegisterServiceAccountStateChange.IServiceAccountStateChange;

using ISourceServiceAccountStateChange = Modules.Sync.Application.Emails.IServiceAccountStateChange;

namespace Modules.Sync.Infrastructure.Emails;

internal sealed class ContextServiceAccountPersistenceClient(IRequestClient<RegisterMailboxStateChangeRequest> requestClient
	, IServiceAccountContextAccessor accountContextAccessor)
	: IContextServiceAccountPersistenceClient
{
	public async Task<Result> SendChangesAsync(IReadOnlyList<ISourceServiceAccountStateChange> sourceChanges, CancellationToken cancellationToken = default)
	{
		var changes = sourceChanges.Select<ISourceServiceAccountStateChange, ITargetServiceAccountStateChange>(artifact =>
			artifact switch
			{
				EmailsConversationArtifactStateChange emailsConversationChanged =>
					new EmailsConversationStateChange(emailsConversationChanged.Id
						, emailsConversationChanged.FolderIds),
				_ => throw new UnreachableException($"{artifact.GetType()} artifact type is not supported"),
			}).ToList();

		var account = accountContextAccessor.Account;

		var request = new RegisterMailboxStateChangeRequest(account.HubId, changes);
		var response = await requestClient.GetResponse<RegisterMailboxStateChangeResponse, Error>(request, cancellationToken);

		if (response.Is<RegisterMailboxStateChangeResponse>(out var _))
		{
			return Result.Success();
		}

		if (response.Is<Error>(out var errorResponse))
		{
			return Result.Failure(errorResponse.Message);
		}

		throw new UnreachableException();
	}
}
