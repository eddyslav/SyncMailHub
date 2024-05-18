namespace Modules.Hub.IntegrationEvents;

public sealed record ServiceAccountAddedIntegrationEvent(Guid Id
	, DateTimeOffset OccuredAt
	, Guid AccountId
	, string AccountEmailAddress
	, Guid UserId
	, string UserEmailAddress
	, string UserFirstName
	, string UserLastName) : IntegrationEvent(Id, OccuredAt);
