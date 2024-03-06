namespace Modules.Hub.Domain.ServiceAccounts;

public abstract class ServiceAccount(ServiceAccountId id) : AggregateRoot<ServiceAccountId>(id), ICreatableEntity
{
	public UserId UserId { get; protected init; } = default!;

	public string EmailAddress { get; protected init; } = default!;

	public string ExternalId { get; protected init; } = default!;

	public DateTimeOffset CreatedAt { get; private init; }

	public void DeleteAccount() =>
		RaiseDomainEvent(new ServiceAccountDeletedDomainEvent(Guid.NewGuid()
			, SystemTimeProvider.UtcNow
			, Id
			, EmailAddress));
}
