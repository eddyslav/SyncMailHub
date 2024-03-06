namespace Modules.Hub.Domain;

// weird dependency, should i try to use here IDateTimeProvider?
internal static class SystemTimeProvider
{
	public static DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
