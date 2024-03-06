using Shared.Utils;

using Persistence.Outbox;

namespace Persistence.Interceptors;

public sealed class TransformDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
{
	private static IEnumerable<OutboxMessage> CreateOutboxMessages(DbContext dbContext) =>
		dbContext.ChangeTracker
			.Entries<IAggregateRoot>()
			.SelectMany(entry => entry.Entity
				.GetDomainEvents()
				.TapAction(entry.Entity.ClearDomainEvents)
				, (_, domainEvent) => OutboxMessage.Create(domainEvent))
			.ToList();

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		if (eventData.Context is not DbContext dbContext)
		{
			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		var outboxMessages = CreateOutboxMessages(dbContext);

		dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}
