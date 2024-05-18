namespace Application.EventBus;

public abstract class IntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler<TIntegrationEvent>, IIntegrationEventHandler
	where TIntegrationEvent : IIntegrationEvent
{
	public abstract Task Handle(TIntegrationEvent integrationEvent, CancellationToken cancellationToken);

	public Task Handle(IIntegrationEvent integrationEvent, CancellationToken cancellationToken) =>
		Handle((TIntegrationEvent)integrationEvent, cancellationToken);
}
