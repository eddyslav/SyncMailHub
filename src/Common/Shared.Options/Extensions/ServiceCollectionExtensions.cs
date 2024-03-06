namespace Shared.Options.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddServiceOptions<TConfiguration>(this IServiceCollection services
		, Action<TConfiguration> configureOptions
		, bool validateOnStart = true)
		where TConfiguration : class
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(configureOptions);

		var optionsBuilder = services
			.AddOptions<TConfiguration>()
			.Configure(configureOptions)
			.ValidateFluently();

		if (validateOnStart)
		{
			optionsBuilder = optionsBuilder.ValidateOnStart();
		}

		return optionsBuilder.Services;
	}

	public static IServiceCollection AddServiceOptions<TConfiguration>(this IServiceCollection services
		, IConfiguration configuration
		, string sectionName
		, bool isRequired = true
		, bool validateOnStart = true)
		where TConfiguration : class
	{
		ArgumentNullException.ThrowIfNull(configuration);

		Func<IConfiguration, IConfigurationSection> sectionGetter = isRequired
			? config => config.GetRequiredSection(sectionName)
			: config => config.GetSection(sectionName);

		return services.AddServiceOptions<TConfiguration>(sectionGetter(configuration).Bind, validateOnStart);
	}

	public static IServiceCollection AddServiceOptions<TConfiguration, TConfigurationValidator>(
		this IServiceCollection services
		, IConfiguration configuration
		, string sectionName
		, bool isRequired = true
		, bool validateOnStart = true)
		where TConfiguration : class
		where TConfigurationValidator : class, IValidator<TConfiguration>
	{
		return services
			.AddSingleton<IValidator<TConfiguration>, TConfigurationValidator>()
			.AddServiceOptions<TConfiguration>(configuration, sectionName, isRequired, validateOnStart);
	}

	public static IServiceCollection AddServiceOptions<TConfigurationService, TConfiguration, TConfigurationValidator>(
		this IServiceCollection services
		, IConfiguration configuration
		, string sectionName
		, bool validateOnStart = true)
		where TConfigurationService : class
		where TConfiguration : class, TConfigurationService
		where TConfigurationValidator : class, IValidator<TConfiguration>
	{
		return services.AddServiceOptions<TConfiguration, TConfigurationValidator>(configuration, sectionName, true /* need to figure out a way how to do this */, validateOnStart)
			.AddSingleton<TConfigurationService>(serviceProvider =>
			{
				var options = serviceProvider.GetRequiredService<IOptions<TConfiguration>>();

				return options.GetConfiguration();
			});
	}
}
