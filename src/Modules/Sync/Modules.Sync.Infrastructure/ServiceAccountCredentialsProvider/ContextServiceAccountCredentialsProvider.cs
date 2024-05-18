namespace Modules.Sync.Infrastructure.ServiceAccountCredentialsProvider;

internal sealed class ContextServiceAccountCredentialsProvider(IServiceAccountCredentialsProvider credentialsProvider
	, IServiceAccountContextAccessor accountContextAccessor)
	: IContextServiceAccountCredentialsProvider
{
	public Task<Result<IServiceAccountCredentials>> GetCredentialsAsync(CancellationToken cancellationToken = default) =>
		Result.Create(accountContextAccessor.Account)
			.Bind(account => credentialsProvider.GetByServiceAccountIdAsync(account.HubId, cancellationToken));
}
