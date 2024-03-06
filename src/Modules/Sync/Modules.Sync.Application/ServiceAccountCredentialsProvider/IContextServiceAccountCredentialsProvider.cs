namespace Modules.Sync.Application.ServiceAccountCredentialsProvider;

public interface IContextServiceAccountCredentialsProvider
{
	Task<Result<IServiceAccountCredentials>> GetCredentialsAsync(CancellationToken cancellationToken = default);
}
