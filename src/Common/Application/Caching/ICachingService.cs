namespace Application.Caching;

public interface ICachingService
{
	Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
		where T : class;

	Task SetAsync<T>(string key
		, T value
		, CacheEntryOptions options
		, CancellationToken cancellationToken = default)
		where T : class;

	Task RemoveAsync(string key, CancellationToken cancellationToken = default);

	Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);
}
