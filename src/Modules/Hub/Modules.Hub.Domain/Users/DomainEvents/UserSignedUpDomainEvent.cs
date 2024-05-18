namespace Modules.Hub.Domain.Users.DomainEvents;

public sealed record UserSignedUpDomainEvent(Guid Id
	, DateTimeOffset OccuredAt
	, UserId UserId
	, string EmailAddress
	, string FirstName
	, string LastName) : DomainEvent(Id, OccuredAt);
