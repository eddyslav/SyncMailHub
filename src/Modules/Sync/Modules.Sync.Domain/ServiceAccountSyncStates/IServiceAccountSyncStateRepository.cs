namespace Modules.Sync.Domain.ServiceAccountSyncStates;

public interface IServiceAccountSyncStateRepository
{
	Task<ServiceAccountSyncState?> GetByAccountIdAsync(ServiceAccountId accountId, CancellationToken cancellationToken = default);

	void Add(ServiceAccountSyncState syncState);

	void Update(ServiceAccountSyncState syncState);
}
