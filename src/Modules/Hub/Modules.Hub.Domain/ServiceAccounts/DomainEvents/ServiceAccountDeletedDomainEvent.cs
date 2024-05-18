namespace Modules.Hub.Domain.ServiceAccounts.DomainEvents;

public sealed record ServiceAccountDeletedDomainEvent(Guid Id
	, DateTimeOffset OccuredAt
	, ServiceAccountId AccountId
	, string EmailAddress)
	: DomainEvent(Id, OccuredAt);
