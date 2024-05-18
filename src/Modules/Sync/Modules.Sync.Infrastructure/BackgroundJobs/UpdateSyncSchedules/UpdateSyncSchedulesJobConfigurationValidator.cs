namespace Modules.Sync.Infrastructure.BackgroundJobs.UpdateSyncSchedules;

internal sealed class UpdateSyncSchedulesJobConfigurationValidator : AbstractValidator<UpdateSyncSchedulesJobConfiguration>
{
	public UpdateSyncSchedulesJobConfigurationValidator()
	{
		RuleFor(x => x.Schedule).GreaterThan(TimeSpan.Zero);
	}
}
