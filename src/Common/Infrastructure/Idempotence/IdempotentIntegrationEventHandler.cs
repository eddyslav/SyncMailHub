namespace Infrastructure.Idempotence;

internal sealed class IdempotentIntegrationEventHandler<TDbContext, TIntegrationEvent>(TDbContext dbContext, IIntegrationEventHandler<TIntegrationEvent> integrationEventHandler)
	: IntegrationEventHandler<TIntegrationEvent>
	where TDbContext : DbContext
	where TIntegrationEvent : IIntegrationEvent
{
	private Task<bool> IsInboxMessageConsumedAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken) =>
		dbContext.Set<InboxMessageConsumer>().AnyAsync(x => x.Id == integrationEvent.Id && x.Name == integrationEventHandler.GetType().FullName, cancellationToken);

	private Task SaveInboxMessageConsumedAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken)
	{
		var entity = new InboxMessageConsumer
		{
			Id = integrationEvent.Id,
			Name = integrationEventHandler.GetType().FullName!,
		};

		dbContext.Set<InboxMessageConsumer>().Add(entity);
		return dbContext.SaveChangesAsync(cancellationToken);
	}

	public override async Task HandleAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken)
	{
		if (await IsInboxMessageConsumedAsync(integrationEvent, cancellationToken))
		{
			return;
		}

		await integrationEventHandler.HandleAsync(integrationEvent, cancellationToken);

		await SaveInboxMessageConsumedAsync(integrationEvent, cancellationToken);
	}
}
