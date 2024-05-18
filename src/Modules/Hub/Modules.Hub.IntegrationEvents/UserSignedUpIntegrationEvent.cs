namespace Modules.Hub.IntegrationEvents;

public sealed record class UserSignedUpIntegrationEvent(Guid Id
	, DateTimeOffset OccuredAt
	, Guid UserId
	, string EmailAddress
	, string FirstName
	, string LastName) : IntegrationEvent(Id, OccuredAt);
