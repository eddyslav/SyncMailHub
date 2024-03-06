namespace Shared.Options.Extensions;

public static class OptionsBuilderExtensions
{
	private static IValidator<TOptions> GetValidator<TOptions>(this IServiceProvider serviceProvider) =>
		serviceProvider.GetService<IValidator<TOptions>>()
			?? throw new InvalidOperationException($"Validator for {typeof(TOptions).Name} was not registered in DI container");

	public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder)
		where TOptions : class
	{
		ArgumentNullException.ThrowIfNull(optionsBuilder);

		var services = optionsBuilder.Services;

		services.AddSingleton<IValidateOptions<TOptions>>(
			serviceProvider => new FluentValidateOptions<TOptions>(optionsBuilder.Name
				, serviceProvider.GetValidator<TOptions>()));

		return optionsBuilder;
	}
}
