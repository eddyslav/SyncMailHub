namespace Modules.Hub.Communication.GetAccountCredentials;

public sealed record GoogleGetAccountCredentialsResponse(string ClientId
	, string ClientSecret
	, string RefreshToken) : GetAccountCredentialsResponseBase;
