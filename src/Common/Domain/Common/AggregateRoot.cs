namespace Domain.Common;

public abstract class AggregateRoot<TKey>(TKey id) : Entity<TKey>(id), IAggregateRoot
	where TKey : notnull, IEntityId
{
	private List<IDomainEvent>? domainEvents;

	public IReadOnlyList<IDomainEvent> GetDomainEvents() => (IReadOnlyList<IDomainEvent>?)domainEvents?.ToList() ?? [];

	public void ClearDomainEvents() => domainEvents?.Clear();

	public void RaiseDomainEvent(IDomainEvent domainEvent)
	{
		domainEvents ??= [];
		domainEvents.Add(domainEvent);
	}
}
