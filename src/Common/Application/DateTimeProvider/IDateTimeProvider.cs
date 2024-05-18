namespace Application.DateTimeProvider;

public interface IDateTimeProvider
{
	DateTimeOffset UtcNow { get; }
}
