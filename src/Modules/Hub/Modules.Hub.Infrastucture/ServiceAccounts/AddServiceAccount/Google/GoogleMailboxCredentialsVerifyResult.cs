namespace Modules.Hub.Infrastucture.ServiceAccounts.AddServiceAccount.Google;

public sealed record GoogleMailboxCredentialsVerifyResult(string EmailAddress
	, string ExternalId
	, string RefreshToken);
