namespace Modules.Notifications.Infrastructure.BackgroundJobs.ProcessInboxMessages;

internal sealed class ProcessInboxMessagesConfigurationValidator : AbstractValidator<ProcessInboxMessagesConfiguration>
{
	public ProcessInboxMessagesConfigurationValidator()
	{
		RuleFor(x => x.BatchSize).GreaterThan(0);
	}
}
