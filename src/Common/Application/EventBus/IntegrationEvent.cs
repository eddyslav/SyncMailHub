namespace Application.EventBus;

public abstract record IntegrationEvent(Guid Id, DateTimeOffset OccuredAt) : IIntegrationEvent;
