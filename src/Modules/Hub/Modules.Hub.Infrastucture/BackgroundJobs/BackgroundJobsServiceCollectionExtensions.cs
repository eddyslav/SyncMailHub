using Modules.Hub.Infrastucture.BackgroundJobs.ProcessOutboxMessages;

namespace Modules.Hub.Infrastucture.BackgroundJobs;

internal static class BackgroundJobsServiceCollectionExtensions
{
	private static readonly string sectionName = "BackgroundJobs";

	public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
	{
		var configurationSection = configuration.GetRequiredSection(sectionName);

		return services.AddProcessOutboxMessagesJob(configurationSection);
	}
}
