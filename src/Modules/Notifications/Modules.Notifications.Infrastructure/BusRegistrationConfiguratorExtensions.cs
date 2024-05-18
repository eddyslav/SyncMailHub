using MassTransit;

using Modules.Hub.IntegrationEvents;

namespace Modules.Notifications.Infrastructure;

public static class BusRegistrationConfiguratorExtensions
{
	public static IBusRegistrationConfigurator AddNotificationsConsumers(this IBusRegistrationConfigurator configurator) =>
		configurator.AddIntegrationEventConsumers<NotificationsDbContext>(typeof(UserSignedUpIntegrationEvent), typeof(ServiceAccountAddedIntegrationEvent));
}
