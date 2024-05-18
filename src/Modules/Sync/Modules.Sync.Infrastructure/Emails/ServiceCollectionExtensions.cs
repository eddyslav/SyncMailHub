namespace Modules.Sync.Infrastructure.Emails;

internal static class ServiceCollectionExtensions
{
	private const string googleSectionName = "GoogleSyncSession";

	public static IServiceCollection AddMailServices(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<Google.ServiceAccountSyncSessionConfiguration, Google.ServiceAccountSyncSessionConfigurationValidator>(configuration, googleSectionName)
			.AddScoped<IContextServiceAccountSyncSessionFactory, ContextServiceAccountSyncSessionFactory>()
			.AddScoped<IContextServiceAccountPersistenceClient, ContextServiceAccountPersistenceClient>();
}
