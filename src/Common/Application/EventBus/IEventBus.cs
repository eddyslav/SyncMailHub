namespace Application.EventBus;

public interface IEventBus
{
	Task PublishAsync<T>(T message, CancellationToken cancellationToken)
		where T : IIntegrationEvent;
}
