using Domain.Common;

using Persistence.Outbox;

namespace Infrastructure.Idempotence;

internal sealed class IdempotentDomainEventHandler<TDbContext, TDomainEvent>(TDbContext dbContext, IDomainEventHandler<TDomainEvent> domainEventHandler)
	: IDomainEventHandler<TDomainEvent>
	where TDbContext : DbContext
	where TDomainEvent : IDomainEvent
{
	private Task<bool> IsOutboxMessageConsumedAsync(TDomainEvent domainEvent, CancellationToken cancellationToken) =>
		dbContext.Set<OutboxMessageConsumer>().AnyAsync(x => x.Id == domainEvent.Id && x.Name == domainEventHandler.GetType().FullName, cancellationToken);

	private Task SaveOutboxMessageConsumedAsync(TDomainEvent domainEvent, CancellationToken cancellationToken)
	{
		var entity = new OutboxMessageConsumer
		{
			Id = domainEvent.Id,
			Name = domainEventHandler.GetType().FullName!,
		};

		dbContext.Set<OutboxMessageConsumer>().Add(entity);
		return dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken)
	{
		if (await IsOutboxMessageConsumedAsync(domainEvent, cancellationToken))
		{
			return;
		}

		await domainEventHandler.Handle(domainEvent, cancellationToken);

		await SaveOutboxMessageConsumedAsync(domainEvent, cancellationToken);
	}
}
