namespace Application.Caching.Extensions;

public static class FluentCacheEntryExtensions
{
	public static CacheEntryOptions SetAbsoluteExpiration(this CacheEntryOptions entry, DateTimeOffset expirationDate)
	{
		entry.AbsoluteExpiration = expirationDate;
		return entry;
	}

	public static CacheEntryOptions SetAbsoluteExpirationRelativeToNow(this CacheEntryOptions entry, TimeSpan expirationTime)
	{
		if (expirationTime <= TimeSpan.Zero)
		{
			throw new ArgumentOutOfRangeException(nameof(expirationTime), expirationTime, "The relative expiration value must be positive");
		}

		entry.AbsoluteExpirationRelativeToNow = expirationTime;
		return entry;
	}

	public static CacheEntryOptions SetSlidingExpiration(this CacheEntryOptions entry, TimeSpan expirationTime)
	{
		if (expirationTime <= TimeSpan.Zero)
		{
			throw new ArgumentOutOfRangeException(nameof(expirationTime), expirationTime, "The relative expiration value must be positive");
		}

		entry.SlidingExpiration = expirationTime;
		return entry;
	}
}
