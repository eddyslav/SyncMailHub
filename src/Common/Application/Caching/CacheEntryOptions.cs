namespace Application.Caching;

public sealed class CacheEntryOptions
{
	private TimeSpan? absoluteExpirationRelativeToNow;
	private TimeSpan? slidingExpiration;

	public DateTimeOffset? AbsoluteExpiration { get; set; }

	public TimeSpan? AbsoluteExpirationRelativeToNow
	{
		get => absoluteExpirationRelativeToNow;
		set
		{
			if (value <= TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException(nameof(AbsoluteExpirationRelativeToNow), value, "The relative expiration value must be positive");
			}

			absoluteExpirationRelativeToNow = value;
		}
	}

	public TimeSpan? SlidingExpiration
	{
		get => slidingExpiration;
		set
		{
			if (value <= TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException(nameof(SlidingExpiration), value, "The sliding expiration value must be positive.");
			}

			slidingExpiration = value;
		}
	}
}
