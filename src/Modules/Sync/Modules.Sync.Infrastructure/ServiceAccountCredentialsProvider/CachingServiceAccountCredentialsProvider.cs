using Application.Caching.Extensions;

namespace Modules.Sync.Infrastructure.ServiceAccountCredentialsProvider;

internal sealed class CachingServiceAccountCredentialsProvider(ICachingService cachingService
	, IServiceAccountCredentialsProvider credentialsProvider
	, IOptions<CachingServiceAccountCredentialsProviderConfiguration> options) : IServiceAccountCredentialsProvider
{
	private readonly CachingServiceAccountCredentialsProviderConfiguration configuration = options.GetConfiguration();

	public Task<Result<IServiceAccountCredentials>> GetByServiceAccountIdAsync(Guid hubId, CancellationToken cancellationToken = default)
	{
		Task<Result<IServiceAccountCredentials>> GetCredentialsFactory(CacheEntryOptions cacheEntry)
		{
			cacheEntry.SetAbsoluteExpirationRelativeToNow(configuration.CacheLifetime);

			return credentialsProvider.GetByServiceAccountIdAsync(hubId, cancellationToken);
		}

		var cacheKey = $"sync/account-creds/{hubId}";
		return cachingService.GetOrAddAsync(cacheKey
			, GetCredentialsFactory
			, cancellationToken);
	}
}
