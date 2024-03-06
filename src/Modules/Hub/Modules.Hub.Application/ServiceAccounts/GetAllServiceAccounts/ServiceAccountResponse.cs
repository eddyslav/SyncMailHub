namespace Modules.Hub.Application.ServiceAccounts.GetAllServiceAccounts;

public sealed record ServiceAccountResponse(ServiceAccountId Id
	, string EmailAddress
	, DateTimeOffset CreatedAt);
