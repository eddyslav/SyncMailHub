namespace Modules.Sync.Persistence.Repositories;

public sealed class ServiceAccountSyncStateRepository(SyncDbContext dbContext) : IServiceAccountSyncStateRepository
{
	public async Task<ServiceAccountSyncState?> GetByAccountIdAsync(ServiceAccountId accountId, CancellationToken cancellationToken) =>
		await dbContext.SyncStates
			.FirstOrDefaultAsync(x => x.AccountId == accountId, cancellationToken);

	public void Add(ServiceAccountSyncState syncState) => dbContext.SyncStates.Add(syncState);

	public void Update(ServiceAccountSyncState syncState) => dbContext.SyncStates.Update(syncState);
}
