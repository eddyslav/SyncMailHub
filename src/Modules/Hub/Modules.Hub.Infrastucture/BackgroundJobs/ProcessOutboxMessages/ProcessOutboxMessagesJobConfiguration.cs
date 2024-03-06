namespace Modules.Hub.Infrastucture.BackgroundJobs.ProcessOutboxMessages;

internal sealed class ProcessOutboxMessagesJobConfiguration : IRecurringJobConfiguration
{
	private static readonly Type type = typeof(ProcessOutboxMessagesJob);

	public string Name => type.FullName!;

	public Type Type => type;

	public TimeSpan Schedule { get; init; }
}
