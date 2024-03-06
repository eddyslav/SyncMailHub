namespace Modules.Hub.Infrastucture.ServiceAccountCredentialsProvider;

internal sealed class CachingServiceAccountCredentialsProviderConfiguration
{
	public bool IsEnabled { get; init; }

	public TimeSpan CacheLifetime { get; init; }
}
