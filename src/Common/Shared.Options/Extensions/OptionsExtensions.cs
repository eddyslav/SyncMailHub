namespace Shared.Options.Extensions;

public static class OptionsExtensions
{
	public static TConfiguration GetConfiguration<TConfiguration>(this IOptions<TConfiguration> options)
		where TConfiguration : class
	{
		ArgumentNullException.ThrowIfNull(options);

		return options.Value ?? throw new InvalidOperationException("No configuration present");
	}
}
