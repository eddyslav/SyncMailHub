namespace Modules.Sync.Infrastructure.BackgroundJobs.UpdateSyncSchedules;

internal sealed class UpdateSyncSchedulesConfiguration
{
	public required int MaxAccountsPerTransaction { get; init; }

	public required TimeSpan JobsSchedule { get; init; }
}
