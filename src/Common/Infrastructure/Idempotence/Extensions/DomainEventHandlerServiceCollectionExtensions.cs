namespace Infrastructure.Idempotence.Extensions;

public static class DomainEventHandlerServiceCollectionExtensions
{
	public static IServiceCollection DecorateDomainEventHandlersWithIdempotency<TDbContext>(this IServiceCollection services, Assembly domainEventHandlersAssembly)
		where TDbContext : DbContext
	{
		domainEventHandlersAssembly.GetTypes()
			.Where(EventHandlersUtils.ImplementsDomainEventHandler)
			.ForEach(domainEventHandlerType =>
			{
				var closedNotificationHandler = domainEventHandlerType
					.GetInterfaces()
					.First(EventHandlersUtils.IsNotificationHandler);

				var genericArgument = closedNotificationHandler.GetGenericArguments().Single();

				var closedIdempotentHandler = typeof(IdempotentDomainEventHandler<,>).MakeGenericType(typeof(TDbContext), genericArgument);

				services.Decorate(closedNotificationHandler, closedIdempotentHandler);
			});

		return services;
	}
}
