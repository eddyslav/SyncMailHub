using Modules.Hub.IntegrationEvents;

namespace Modules.Sync.Infrastructure;

public static class BusRegistrationConfiguratorExtensions
{
	public static IBusRegistrationConfigurator AddSyncConsumers(this IBusRegistrationConfigurator configurator) =>
		configurator.AddIntegrationEventConsumers<SyncDbContext>(typeof(ServiceAccountAddedIntegrationEvent), typeof(ServiceAccountDeletedIntegrationEvent))
			.TapAction(configurator => configurator.AddRequestClient<GetAccountCredentialsRequest>());
}
