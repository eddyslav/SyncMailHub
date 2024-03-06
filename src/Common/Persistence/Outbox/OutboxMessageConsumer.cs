namespace Persistence.Outbox;

public sealed class OutboxMessageConsumer
{
	public required Guid Id { get; init; }

	public required string Name { get; init; }
}
