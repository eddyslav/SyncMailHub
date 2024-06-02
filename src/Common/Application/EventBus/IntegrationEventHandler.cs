namespace Application.EventBus;

public abstract class IntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler<TIntegrationEvent>, IIntegrationEventHandler
	where TIntegrationEvent : IIntegrationEvent
{
	public abstract Task HandleAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken);

	public Task HandleAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken) =>
		HandleAsync((TIntegrationEvent)integrationEvent, cancellationToken);
}
