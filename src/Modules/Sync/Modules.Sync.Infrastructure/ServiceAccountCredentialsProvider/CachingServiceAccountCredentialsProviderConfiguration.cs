namespace Modules.Sync.Infrastructure.ServiceAccountCredentialsProvider;

internal sealed class CachingServiceAccountCredentialsProviderConfiguration
{
	public required bool IsEnabled { get; init; }

	public TimeSpan CacheLifetime { get; init; }
}
