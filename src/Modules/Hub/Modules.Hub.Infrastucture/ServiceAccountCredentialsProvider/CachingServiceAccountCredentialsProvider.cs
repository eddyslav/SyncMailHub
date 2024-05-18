namespace Modules.Hub.Infrastucture.ServiceAccountCredentialsProvider;

internal sealed class CachingServiceAccountCredentialsProvider(ICachingService cachingService
	, IServiceAccountCredentialsProvider credentialsProvider
	, IOptions<CachingServiceAccountCredentialsProviderConfiguration> options) : IServiceAccountCredentialsProvider
{
	private readonly CachingServiceAccountCredentialsProviderConfiguration configuration = options.GetConfiguration();

	public Task<Result<IServiceAccountCredentials>> GetByAccountIdAsync(ServiceAccountId accountId, CancellationToken cancellationToken = default)
	{
		Task<Result<IServiceAccountCredentials>> GetCredentialsFactory(CacheEntryOptions cacheEntry)
		{
			cacheEntry.SetAbsoluteExpirationRelativeToNow(configuration.CacheLifetime);

			return credentialsProvider.GetByAccountIdAsync(accountId, cancellationToken);
		}

		var cacheKey = string.Format(CacheKeys.AccountCredentialsCacheKeyTemplate, accountId.Value);
		return cachingService.GetOrAddAsync(cacheKey
			, GetCredentialsFactory
			, cancellationToken);
	}
}
