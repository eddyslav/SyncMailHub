namespace Modules.Sync.Application.Emails;

public interface IContextServiceAccountSyncSessionFactory
{
	Task<Result<IServiceAccountSyncSession>> GetSyncSessionAsync(CancellationToken cancellationToken = default);
}
