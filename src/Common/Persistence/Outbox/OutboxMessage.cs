namespace Persistence.Outbox;

public sealed class OutboxMessage
{
	private static readonly JsonSerializerSettings jsonSerializerSettings = new()
	{
		TypeNameHandling = TypeNameHandling.All,
	};

	public required Guid Id { get; init; }

	public required DateTimeOffset OccuredAt { get; init; }

	public required string Type { get; init; }

	public required string Content { get; init; }

	public DateTimeOffset? ProcessedAt { get; set; }

	public string? ErrorMessage { get; set; }

	public static OutboxMessage Create<TDomainEvent>(TDomainEvent domainEvent)
		where TDomainEvent : IDomainEvent
	{
		var typeName = domainEvent.GetType().FullName!;
		var content = JsonConvert.SerializeObject(domainEvent, jsonSerializerSettings);

		return new OutboxMessage
		{
			Id = domainEvent.Id,
			OccuredAt = domainEvent.OccuredAt,
			Type = typeName!,
			Content = content,
		};
	}
}
