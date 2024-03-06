namespace Modules.Sync.Infrastructure.BackgroundJobs.ProcessMailboxEmails;

internal sealed class ProcessMailboxEmailsJobConfiguration : IRecurringJobConfiguration
{
	private static readonly Type type = typeof(ProcessMailboxEmailsJob);

	public string Name => type.FullName!;

	public Type Type => type;

	public required TimeSpan Schedule { get; init; }
}
