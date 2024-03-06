namespace Modules.Sync.Application.ServiceAccountCredentialsProvider;

public sealed record GoogleServiceAccountCredentials(string ClientId
	, string ClientSecret
	, string RefreshToken) : IServiceAccountCredentials;
