namespace Modules.Sync.Infrastructure.BackgroundJobs.ProcessInboxMessages;

internal sealed class ProcessInboxMessagesConfigurationValidator : AbstractValidator<ProcessInboxMessagesConfiguration>
{
	public ProcessInboxMessagesConfigurationValidator()
	{
		RuleFor(x => x.BatchSize).GreaterThan(0);

		RuleFor(x => x.MessageHandlersRetriesCount).GreaterThan(0);
	}
}
