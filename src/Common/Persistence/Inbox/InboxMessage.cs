using Application.EventBus;

namespace Persistence.Inbox;

public sealed class InboxMessage
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

	public static InboxMessage Create<TIntegrationEvent>(TIntegrationEvent integrationEvent)
		where TIntegrationEvent : IIntegrationEvent
	{
		var typeName = integrationEvent.GetType().FullName!;
		var content = JsonConvert.SerializeObject(integrationEvent, jsonSerializerSettings);

		return new InboxMessage
		{
			Id = integrationEvent.Id,
			OccuredAt = integrationEvent.OccuredAt,
			Type = typeName,
			Content = content,
		};
	}
}
