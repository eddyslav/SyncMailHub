namespace Modules.Hub.Infrastucture.BackgroundJobs.ProcessOutboxMessages;

internal sealed class ProcessOutboxMessagesConfiguration
{
	public int BatchSize { get; init; }

	public int MessageHandlersRetriesCount { get; init; }
}
