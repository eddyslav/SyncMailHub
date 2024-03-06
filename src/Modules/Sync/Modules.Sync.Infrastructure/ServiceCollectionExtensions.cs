using Microsoft.Extensions.DependencyInjection.Extensions;

using Infrastructure.DateTimeProvider;

using Modules.Sync.Infrastructure.BackgroundJobs;
using Modules.Sync.Infrastructure.Emails;
using Modules.Sync.Infrastructure.Persistence;
using Modules.Sync.Infrastructure.ServiceAccountCredentialsProvider;

namespace Modules.Sync.Infrastructure;

public static class ServiceCollectionExtensions
{
	private const string configurationSectionName = "Modules:Sync";

	public static IServiceCollection AddSyncModule(this IServiceCollection services, IConfiguration configuration)
	{
		configuration = configuration.GetRequiredSection(configurationSectionName);

		return services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly)
			.DecorateIntegrationEventHandlersWithIdempotency<SyncDbContext>(Application.AssemblyMarker.Assembly)
			.AddBackgroundJobs(configuration)
			.AddMailServices(configuration)
			.AddPersistence(configuration)
			.AddServiceAccountCredentialsProvider(configuration)
			.AddScoped<IContextServiceAccountPersistenceClient, ContextServiceAccountPersistenceClient>()
			.AddSingleton<IServiceAccountContextAccessor, ServiceAccountContextAccessor>()
			.TapAction(services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>);
	}
}
