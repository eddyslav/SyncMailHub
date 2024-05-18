using System.Linq.Expressions;

using System.Runtime.CompilerServices;

namespace Modules.Sync.Persistence.Repositories;

public sealed class ServiceAccountRepository(SyncDbContext dbContext) : IServiceAccountRepository
{
	private async IAsyncEnumerable<IReadOnlyList<ServiceAccount>> GetServiceAccountsAsync(int maxAccountsCount
		, Expression<Func<ServiceAccount, bool>>? filter
		, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		var query = dbContext.ServiceAccounts
			.OrderBy(serviceAccount => serviceAccount.CreatedAt)
			.Take(maxAccountsCount);

		if (filter is not null)
		{
			query = query.Where(filter);
		}
		
		var page = 1;

		while (true)
		{
			var results = await query.Skip((page - 1) * maxAccountsCount)
				.ToListAsync(cancellationToken);

			if (!results.Any())
			{
				break;
			}

			yield return results;

			if (results.Count < maxAccountsCount)
			{
				break;
			}
		}
	}

	public Task<ServiceAccount?> GetByIdAsync(ServiceAccountId id, CancellationToken cancellationToken = default) =>
		dbContext.ServiceAccounts.FirstOrDefaultAsync(account => account.Id == id, cancellationToken);

	public IAsyncEnumerable<IReadOnlyList<ServiceAccount>> GetAllServiceAccountsAsync(int maxAccountsCount, CancellationToken cancellationToken) =>
		GetServiceAccountsAsync(maxAccountsCount, null, cancellationToken);

	public IAsyncEnumerable<IReadOnlyList<ServiceAccount>> GetServiceAccountsCreatedAfterAsync(int maxAccountsCount
		, DateTimeOffset effectiveDate
		, CancellationToken cancellationToken) =>
		GetServiceAccountsAsync(maxAccountsCount
			, account => account.CreatedAt >= effectiveDate
			, cancellationToken);

	public void Add(ServiceAccount account) => dbContext.ServiceAccounts.Add(account);

	public Task<int> RemoveByHubIdAsync(Guid hubId, CancellationToken cancellationToken = default) =>
		dbContext.ServiceAccounts
			.Where(account => account.HubId == hubId)
			.ExecuteDeleteAsync(cancellationToken);
}
