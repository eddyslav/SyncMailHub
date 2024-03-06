namespace Domain.Common;

public interface IAggregateRoot : IEntity
{
	IReadOnlyList<IDomainEvent> GetDomainEvents();
	
	void ClearDomainEvents();
	
	void RaiseDomainEvent(IDomainEvent domainEvent);
}
