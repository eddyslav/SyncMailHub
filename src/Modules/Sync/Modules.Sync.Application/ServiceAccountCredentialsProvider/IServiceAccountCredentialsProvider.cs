namespace Modules.Sync.Application.ServiceAccountCredentialsProvider;

public interface IServiceAccountCredentialsProvider
{
	Task<Result<IServiceAccountCredentials>> GetByServiceAccountIdAsync(Guid hubId, CancellationToken cancellationToken = default);
}
