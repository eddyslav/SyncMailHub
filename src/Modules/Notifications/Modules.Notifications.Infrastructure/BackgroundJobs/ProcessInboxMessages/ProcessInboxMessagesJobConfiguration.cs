namespace Modules.Notifications.Infrastructure.BackgroundJobs.ProcessInboxMessages;

internal sealed class ProcessInboxMessagesJobConfiguration : IRecurringJobConfiguration
{
	private static readonly Type type = typeof(ProcessInboxMessagesJob);

	public string Name => type.FullName!;

	public Type Type => type;

	public required TimeSpan Schedule { get; init; }
}
