using MediatR;

namespace Infrastructure.Idempotence.Extensions;

public static class EventHandlersUtils
{
	private static readonly Type domainEventHandlerType = typeof(IDomainEventHandler<>);
	private static readonly Type notificationEventHandlerType = typeof(INotificationHandler<>);
	private static readonly Type integrationEventHandlerType = typeof(IIntegrationEventHandler<>);

	private static bool IsHandlerType(Type interfaceType, Type handlerType) => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == handlerType;

	public static bool IsNotificationHandler(Type interfaceType) => IsHandlerType(interfaceType, notificationEventHandlerType);

	public static bool IsDomainEventHandler(Type interfaceType) => IsHandlerType(interfaceType, domainEventHandlerType);

	public static bool IsIntegrationEventHandler(Type interfaceType) => IsHandlerType(interfaceType, integrationEventHandlerType);

	public static bool ImplementsDomainEventHandler(Type type) =>
		type.GetInterfaces() is Type[] interfaces
			&& interfaces.Any()
			&& interfaces.All(interfaceType => IsNotificationHandler(interfaceType) || IsDomainEventHandler(interfaceType));

	public static bool ImplementsIntegrationEventHandler(Type type) => type.GetInterfaces().Any(IsIntegrationEventHandler);
}
