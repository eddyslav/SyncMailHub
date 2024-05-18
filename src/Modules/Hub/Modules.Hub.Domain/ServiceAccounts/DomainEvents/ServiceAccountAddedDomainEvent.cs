namespace Modules.Hub.Domain.ServiceAccounts.DomainEvents;

public sealed record ServiceAccountAddedDomainEvent(Guid Id
	, DateTimeOffset OccuredAt
	, ServiceAccountId AccountId
	, string AccountEmailAddress
	, UserId UserId
	, string UserEmailAddress
	, string UserFirstName
	, string UserLastName) : DomainEvent(Id, OccuredAt);
