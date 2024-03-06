using Modules.Notifications.Infrastructure.BackgroundJobs.ProcessInboxMessages;

namespace Modules.Notifications.Infrastructure.BackgroundJobs;

internal static class BackgroundJobsServiceCollectionExtensions
{
	private const string sectionName = "BackgroundJobs";

	public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
	{
		var configurationSection = configuration.GetRequiredSection(sectionName);

		return services.AddProcessInboxMessagesJob(configurationSection);
	}
}
