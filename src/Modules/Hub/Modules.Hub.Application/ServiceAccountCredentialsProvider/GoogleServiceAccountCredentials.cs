namespace Modules.Hub.Application.ServiceAccountCredentialsProvider;

public sealed record GoogleServiceAccountCredentials(string ClientId
	, string ClientSecret
	, string RefreshToken) : IServiceAccountCredentials;
