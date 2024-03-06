namespace Modules.Hub.Infrastucture.BackgroundJobs.ProcessOutboxMessages;

internal sealed class ProcessOutboxMessagesConfigurationValidator : AbstractValidator<ProcessOutboxMessagesConfiguration>
{
	public ProcessOutboxMessagesConfigurationValidator()
	{
		RuleFor(x => x.BatchSize).GreaterThan(0);
	}
}
