using Microsoft.Extensions.DependencyInjection.Extensions;

using Persistence.Interceptors;

using Modules.Sync.Domain;

using Modules.Sync.Persistence.Repositories;

namespace Modules.Sync.Infrastructure.Persistence;

internal static class ServiceCollectionExtensions
{
	private static readonly string connectionStringSettingName = "SyncDb";

	public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) =>
		services.AddDbContext<SyncDbContext>((serviceProvider, options) =>
			options.UseNpgsql(configuration.GetConnectionString(connectionStringSettingName)
				, optionsBuilder => optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", SchemaNames.Sync))
				.AddInterceptors(serviceProvider.GetRequiredService<TrackEntitiesInterceptor>()
					, serviceProvider.GetRequiredService<TransformDomainEventsToOutboxMessagesInterceptor>()))
			.AddScoped<IUnitOfWork, UnitOfWork>()
			.AddScoped<IServiceAccountRepository, ServiceAccountRepository>()
			.AddScoped<IServiceAccountSyncStateRepository, ServiceAccountSyncStateRepository>()
			.TapAction(services.TryAddSingleton<TrackEntitiesInterceptor>);
}
