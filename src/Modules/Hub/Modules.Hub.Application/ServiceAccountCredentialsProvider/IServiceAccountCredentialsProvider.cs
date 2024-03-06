namespace Modules.Hub.Application.ServiceAccountCredentialsProvider;

public interface IServiceAccountCredentialsProvider
{
	Task<Result<IServiceAccountCredentials>> GetByAccountIdAsync(ServiceAccountId accountId, CancellationToken cancellationToken = default);
}
