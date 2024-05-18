namespace Modules.Sync.Infrastructure.BackgroundJobs.ProcessInboxMessages;

internal sealed class ProcessInboxMessagesJobConfigurationValidator : AbstractValidator<ProcessInboxMessagesJobConfiguration>
{
	public ProcessInboxMessagesJobConfigurationValidator()
	{
		RuleFor(x => x.Schedule).GreaterThan(TimeSpan.Zero);
	}
}
