namespace Application.Caching.Extensions;

public static class CachingServiceExtensions
{
	public static async Task<T> GetOrAddAsync<T>(this ICachingService cachingService
		, string key
		, Func<CacheEntryOptions, Task<T>> valueFactory
		, CancellationToken cancellationToken = default)
		where T : class
	{
		var value = await cachingService.GetAsync<T>(key, cancellationToken);

		if (value is null)
		{
			var entryOptions = new CacheEntryOptions();
			value = await valueFactory(entryOptions);

			await cachingService.SetAsync(key, value, entryOptions, cancellationToken);
		}

		return value;
	}

	public static async Task<Result<T>> GetOrAddAsync<T>(this ICachingService cachingService
		, string key
		, Func<CacheEntryOptions, Task<Result<T>>> valueFactory
		, CancellationToken cancellationToken = default)
		where T : class
	{
		var value = await cachingService.GetAsync<T>(key, cancellationToken);

		if (value is null)
		{
			var entryOptions = new CacheEntryOptions();
			var valueResult = await valueFactory(entryOptions);

			return valueResult.Tap(value =>
				cachingService.SetAsync(key
					, value
					, entryOptions
					, cancellationToken));
		}

		return value;
	}
}
