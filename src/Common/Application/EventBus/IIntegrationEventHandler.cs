namespace Application.EventBus;

public interface IIntegrationEventHandler<in TIntegrationEvent>
	where TIntegrationEvent : IIntegrationEvent
{
	Task Handle(TIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}

public interface IIntegrationEventHandler
{
	Task Handle(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}
