namespace Modules.Sync.Infrastructure.ServiceAccountCredentialsProvider;

internal static class ServiceCollectionExtensions
{
	private static readonly string sectionName = "ServiceAccountCredentialsProvider";

	public static IServiceCollection AddServiceAccountCredentialsProvider(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<ServiceAccountCredentialsProviderConfiguration, ServiceAccountCredentialsProviderConfigurationValidator>(configuration, sectionName)
			.AddScoped<HubServiceAccountCredentialsProvider>()
			.AddScoped<IServiceAccountCredentialsProvider>(serviceProvider =>
			{
				var cachingConfiguration = serviceProvider.GetRequiredService<IOptions<ServiceAccountCredentialsProviderConfiguration>>()
					.GetConfiguration()
					.CachingConfiguration;

				var baseProvider = serviceProvider.GetRequiredService<HubServiceAccountCredentialsProvider>();

				return cachingConfiguration?.IsEnabled ?? false
					? new CachingServiceAccountCredentialsProvider(serviceProvider.GetRequiredService<ICachingService>()
						, baseProvider
						, new OptionsWrapper<CachingServiceAccountCredentialsProviderConfiguration>(cachingConfiguration))
					: baseProvider;
			})
			.AddScoped<IContextServiceAccountCredentialsProvider, ContextServiceAccountCredentialsProvider>();
}
