using Modules.Sync.Infrastructure.BackgroundJobs.ProcessInboxMessages;
using Modules.Sync.Infrastructure.BackgroundJobs.UpdateSyncSchedules;

namespace Modules.Sync.Infrastructure.BackgroundJobs;

internal static class ServiceCollectionExtensions
{
	private static readonly string sectionName = "BackgroundJobs";

	public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
	{
		var configurationSection = configuration.GetRequiredSection(sectionName);

		return services.AddProcessInboxMessagesJob(configurationSection)
			.AddUpdateSyncSchedulesJob(configurationSection);
	}
}
