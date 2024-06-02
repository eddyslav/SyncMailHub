namespace Modules.Hub.Persistence.Repositories;

public sealed class ServiceAccountRepository(HubDbContext dbContext) : IServiceAccountRepository
{
	public Task<bool> CheckIfExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken = default) =>
		dbContext.ServiceAccounts.AnyAsync(account => account.ExternalId == externalId, cancellationToken);

	public Task<bool> CheckIfExistsByIdAndUserIdAsync(ServiceAccountId id, UserId userId, CancellationToken cancellationToken = default) =>
		dbContext.ServiceAccounts.AnyAsync(account => account.Id == id && account.UserId == userId, cancellationToken);

	public Task<ServiceAccount?> GetByIdAsync(ServiceAccountId id, CancellationToken cancellationToken = default) =>
		dbContext.ServiceAccounts
			.FirstOrDefaultAsync(account => account.Id == id, cancellationToken);

	public Task<ServiceAccount?> GetByIdAndUserIdAsync(ServiceAccountId id
		, UserId userId
		, CancellationToken cancellationToken = default) =>
		dbContext.ServiceAccounts
			.FirstOrDefaultAsync(account => account.Id == id && account.UserId == userId, cancellationToken);

	public async Task<IReadOnlyList<ServiceAccount>> GetAllPerUserAsync(UserId userId, CancellationToken cancellationToken = default) =>
		await dbContext.ServiceAccounts
			.Where(account => account.UserId == userId)
			.ToListAsync(cancellationToken);

	public void Add(ServiceAccount account) => dbContext.ServiceAccounts.Add(account);

	public void Remove(ServiceAccount account) => dbContext.ServiceAccounts.Remove(account);
}
