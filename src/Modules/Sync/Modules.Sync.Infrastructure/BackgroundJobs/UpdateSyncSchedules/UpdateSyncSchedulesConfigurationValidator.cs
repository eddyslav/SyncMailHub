namespace Modules.Sync.Infrastructure.BackgroundJobs.UpdateSyncSchedules;

internal sealed class UpdateSyncSchedulesConfigurationValidator : AbstractValidator<UpdateSyncSchedulesConfiguration>
{
	public UpdateSyncSchedulesConfigurationValidator()
	{
		RuleFor(x => x.JobsSchedule).GreaterThan(TimeSpan.Zero);
	}
}
