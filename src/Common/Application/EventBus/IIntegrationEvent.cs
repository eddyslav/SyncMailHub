namespace Application.EventBus;

public interface IIntegrationEvent
{
	Guid Id { get; }

	DateTimeOffset OccuredAt { get; }
}
