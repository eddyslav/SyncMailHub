using System.Collections.Concurrent;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

using Application.Caching;

namespace Infrastructure.Caching;

public sealed class CachingService(IDistributedCache cache) : ICachingService
{
	private static readonly JsonSerializer jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.All,
	});

	private readonly ConcurrentDictionary<string, bool> cacheKeys = new();

	private static T Deserialize<T>(byte[] data)
	{
		using var ms = new MemoryStream(data);
		using var sr = new BsonDataReader(ms);

		return jsonSerializer.Deserialize<T>(sr)!;
	}

	private static byte[] Serialize<T>(T value)
	{
		using var ms = new MemoryStream();
		using (var sw = new BsonDataWriter(ms))
		{
			jsonSerializer.Serialize(sw, value);
		}

		return ms.ToArray();
	}

	public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
		where T : class
	{
		var bytes = await cache.GetAsync(key, cancellationToken);
		if (bytes is not null)
		{
			await cache.RefreshAsync(key, cancellationToken);
			return Deserialize<T>(bytes);
		}

		return default;
	}

	public async Task SetAsync<T>(string key
		, T value
		, CacheEntryOptions options
		, CancellationToken cancellationToken)
		where T : class
	{
		var bytes = Serialize(value);

		await cache.SetAsync(key
			, bytes
			, new DistributedCacheEntryOptions
			{
				AbsoluteExpiration = options.AbsoluteExpiration,
				AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
				SlidingExpiration = options.SlidingExpiration,
			}
			, cancellationToken);

		cacheKeys.TryAdd(key, true);
	}

	public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
	{
		await cache.RemoveAsync(key, cancellationToken);
		cacheKeys.TryRemove(key, out _);
	}

	public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
	{
		var tasks = cacheKeys.Keys
			.Where(key => key.StartsWith(prefix))
			.Select(key => RemoveAsync(key, cancellationToken));

		return Task.WhenAll(tasks);
	}
}
