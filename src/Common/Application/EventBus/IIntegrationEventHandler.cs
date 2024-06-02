namespace Application.EventBus;

public interface IIntegrationEventHandler<in TIntegrationEvent>
	where TIntegrationEvent : IIntegrationEvent
{
	Task HandleAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}

public interface IIntegrationEventHandler
{
	Task HandleAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}
