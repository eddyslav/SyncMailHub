using System.Runtime.CompilerServices;

namespace Modules.Sync.Domain.ServiceAccounts;

public interface IServiceAccountRepository
{
	Task<ServiceAccount?> GetByIdAsync(ServiceAccountId id, CancellationToken cancellationToken = default);

	IAsyncEnumerable<IReadOnlyList<ServiceAccount>> GetAllServiceAccountsAsync(int maxAccountsCount
		, [EnumeratorCancellation] CancellationToken cancellationToken = default);

	IAsyncEnumerable<IReadOnlyList<ServiceAccount>> GetServiceAccountsCreatedAfterAsync(int maxAccountsCount
		, DateTimeOffset effectiveDate
		, [EnumeratorCancellation] CancellationToken cancellationToken = default);

	void Add(ServiceAccount account);

	Task<int> RemoveByHubIdAsync(Guid hubId, CancellationToken cancellationToken = default);
}
