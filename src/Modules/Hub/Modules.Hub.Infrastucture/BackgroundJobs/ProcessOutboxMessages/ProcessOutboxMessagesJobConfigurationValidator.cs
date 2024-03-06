namespace Modules.Hub.Infrastucture.BackgroundJobs.ProcessOutboxMessages;

internal sealed class ProcessOutboxMessagesJobConfigurationValidator : AbstractValidator<ProcessOutboxMessagesJobConfiguration>
{
	public ProcessOutboxMessagesJobConfigurationValidator()
	{
		RuleFor(x => x.Schedule).GreaterThan(TimeSpan.Zero);
	}
}
