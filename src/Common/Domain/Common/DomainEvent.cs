namespace Domain.Common;

public abstract record DomainEvent(Guid Id, DateTimeOffset OccuredAt) : IDomainEvent;
