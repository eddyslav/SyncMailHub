namespace Infrastructure.Idempotence.Extensions;

public static class IntegrationEventConsumerBusConfiguratorExtensions
{
	public static IBusRegistrationConfigurator AddIntegrationEventConsumers<TDbContext>(this IBusRegistrationConfigurator busConfigurator
		, params Type[] integrationEventTypes)
		where TDbContext : DbContext =>
		busConfigurator.TapAction(() =>
			integrationEventTypes.ForEach(integrationEventType =>
				busConfigurator.AddConsumer(typeof(IntegrationEventConsumer<,>).MakeGenericType(typeof(TDbContext), integrationEventType))));
}
