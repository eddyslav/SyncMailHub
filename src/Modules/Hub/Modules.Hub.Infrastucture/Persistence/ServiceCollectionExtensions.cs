using Persistence.Interceptors;

using Modules.Hub.Persistence.Repositories;

namespace Modules.Hub.Infrastucture.Persistence;

internal static class ServiceCollectionExtensions
{
	private static readonly string connectionStringSettingName = "HubDb";

	public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) =>
		services.AddDbContext<HubDbContext>((serviceProvider, options) =>
			options.UseNpgsql(configuration.GetConnectionString(connectionStringSettingName)
				, optionsBuilder => optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", SchemaNames.Hub))
			.AddInterceptors(serviceProvider.GetRequiredService<TrackEntitiesInterceptor>()
				, serviceProvider.GetRequiredService<TransformDomainEventsToOutboxMessagesInterceptor>()))
			.AddScoped<IUnitOfWork, UnitOfWork>()
			.AddScoped<IServiceAccountRepository, ServiceAccountRepository>()
			.AddScoped<IUserRepository, UserRepository>()
			.TapAction(services.TryAddSingleton<TrackEntitiesInterceptor>)
			.TapAction(services.TryAddSingleton<TransformDomainEventsToOutboxMessagesInterceptor>);
}
