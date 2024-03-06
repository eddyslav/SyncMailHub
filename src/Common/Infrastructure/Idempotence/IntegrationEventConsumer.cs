namespace Infrastructure.Idempotence;

internal sealed class IntegrationEventConsumer<TDbContext, TIntegrationEvent>(TDbContext dbContext) : IConsumer<TIntegrationEvent>
	where TDbContext : DbContext
	where TIntegrationEvent : class, IIntegrationEvent
{
	public Task Consume(ConsumeContext<TIntegrationEvent> context)
	{
		var integrationEvent = context.Message;

		var inboxMessage = InboxMessage.Create(integrationEvent);

		dbContext.Set<InboxMessage>().Add(inboxMessage);
		return dbContext.SaveChangesAsync(context.CancellationToken);
	}
}
