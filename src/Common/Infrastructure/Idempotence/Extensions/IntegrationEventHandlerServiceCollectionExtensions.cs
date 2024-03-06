namespace Infrastructure.Idempotence.Extensions;

public static class IntegrationEventHandlerServiceCollectionExtensions
{
	public static IServiceCollection DecorateIntegrationEventHandlersWithIdempotency<TDbContext>(this IServiceCollection services, Assembly integrationEventHandlersAssembly)
		where TDbContext : DbContext =>
		services.TapAction(services => integrationEventHandlersAssembly.GetTypes()
			.Where(EventHandlersUtils.ImplementsIntegrationEventHandler)
			.ForEach(integrationEventHandlerType =>
			{
				var closedIntegrationEventHandler = integrationEventHandlerType
					.GetInterfaces()
					.First(EventHandlersUtils.IsIntegrationEventHandler);

				var genericArgument = closedIntegrationEventHandler.GetGenericArguments().Single();

				var closedIdempotentHandler = typeof(IdempotentIntegrationEventHandler<,>).MakeGenericType(typeof(TDbContext), genericArgument);

				services.AddScoped(integrationEventHandlerType);
				services.Decorate(integrationEventHandlerType, closedIdempotentHandler);
			}));
}
