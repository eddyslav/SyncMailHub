namespace Modules.Hub.IntegrationEvents;

public sealed record ServiceAccountDeletedIntegrationEvent(Guid Id
	, DateTimeOffset OccuredAt
	, Guid AccountId
	, string EmailAddress)
	: IntegrationEvent(Id, OccuredAt);
