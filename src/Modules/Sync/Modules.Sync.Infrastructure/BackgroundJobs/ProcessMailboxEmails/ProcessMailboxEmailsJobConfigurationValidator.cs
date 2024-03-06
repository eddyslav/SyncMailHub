namespace Modules.Sync.Infrastructure.BackgroundJobs.ProcessMailboxEmails;

internal sealed class ProcessMailboxEmailsJobConfigurationValidator : AbstractValidator<ProcessMailboxEmailsJobConfiguration>
{
	public ProcessMailboxEmailsJobConfigurationValidator()
	{
		RuleFor(x => x.Schedule).GreaterThan(TimeSpan.Zero);
	}
}
