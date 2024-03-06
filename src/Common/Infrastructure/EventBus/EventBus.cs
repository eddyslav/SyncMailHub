namespace Infrastructure.EventBus;

public sealed class EventBus(IPublishEndpoint endpoint) : IEventBus
{
	public Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : IIntegrationEvent =>
		endpoint.Publish(message, cancellationToken);
}
