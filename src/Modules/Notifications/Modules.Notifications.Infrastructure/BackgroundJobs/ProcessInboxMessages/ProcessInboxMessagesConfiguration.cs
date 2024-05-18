namespace Modules.Notifications.Infrastructure.BackgroundJobs.ProcessInboxMessages;

internal sealed class ProcessInboxMessagesConfiguration
{
	public required int BatchSize { get; init; }

	public required int MessageHandlersRetriesCount { get; init; }
}
