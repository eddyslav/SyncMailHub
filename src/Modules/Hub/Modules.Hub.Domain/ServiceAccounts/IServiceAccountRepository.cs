namespace Modules.Hub.Domain.ServiceAccounts;

public interface IServiceAccountRepository
{
	Task<bool> CheckIfExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);

	Task<bool> CheckIfExistsByIdAndUserIdAsync(ServiceAccountId accountId, UserId userId, CancellationToken cancellationToken);

	// TODO: remove
	Task<ServiceAccount?> GetByIdAsync(ServiceAccountId id, CancellationToken cancellationToken = default);

	Task<ServiceAccount?> GetByIdAndUserIdAsync(ServiceAccountId id
		, UserId userId
		, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<ServiceAccount>> GetAllPerUserAsync(UserId userId, CancellationToken cancellationToken = default);

	void Add(ServiceAccount account);

	void Remove(ServiceAccount account);
}
