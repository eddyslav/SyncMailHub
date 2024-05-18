namespace Modules.Hub.Domain.ServiceAccounts;

public interface IServiceAccountRepository
{
	Task<bool> CheckIfExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);

	Task<bool> CheckIfExistsByIdAndUserIdAsync(ServiceAccountId id, UserId userId, CancellationToken cancellationToken = default);

	Task<ServiceAccount?> GetByIdAsync(ServiceAccountId id, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<ServiceAccount>> GetAllAccountsPerUserAsync(UserId userId, CancellationToken cancellationToken = default);

	void Add(ServiceAccount account);

	void Remove(ServiceAccount account);
}
