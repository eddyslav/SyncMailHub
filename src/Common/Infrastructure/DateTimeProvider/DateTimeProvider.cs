using Application.DateTimeProvider;

namespace Infrastructure.DateTimeProvider;

public sealed class DateTimeProvider : IDateTimeProvider
{
	public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
