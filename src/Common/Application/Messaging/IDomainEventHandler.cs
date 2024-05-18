using Domain.Common;

namespace Application.Messaging;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
	where TDomainEvent : IDomainEvent;
