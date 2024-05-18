namespace Modules.Sync.Infrastructure.BackgroundJobs.UpdateSyncSchedules;

internal sealed class UpdateSyncSchedulesJobConfiguration : IRecurringJobConfiguration
{
	private static readonly Type type = typeof(UpdateSyncSchedulesJob);

	public string Name => type.FullName!;

	public Type Type => type;

	public required TimeSpan Schedule { get; init; }
}
