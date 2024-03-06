namespace Modules.Sync.Infrastructure.BackgroundJobs.UpdateSyncSchedules;

internal static class ServiceCollectionExtensions
{
	private static readonly string sectionName = "UpdateSyncSchedules";

	public static IServiceCollection AddUpdateSyncSchedulesJob(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<UpdateSyncSchedulesConfiguration, UpdateSyncSchedulesConfigurationValidator>(configuration, sectionName)
			.AddServiceOptions<IJobConfiguration, UpdateSyncSchedulesJobConfiguration, UpdateSyncSchedulesJobConfigurationValidator>(configuration, sectionName);
}
