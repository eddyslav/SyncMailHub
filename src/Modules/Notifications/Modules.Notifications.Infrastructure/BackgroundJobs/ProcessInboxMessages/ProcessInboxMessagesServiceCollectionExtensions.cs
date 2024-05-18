namespace Modules.Notifications.Infrastructure.BackgroundJobs.ProcessInboxMessages;

internal static class ProcessInboxMessagesServiceCollectionExtensions
{
	private static readonly string sectionName = "ProcessInboxMessages";

	public static IServiceCollection AddProcessInboxMessagesJob(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<ProcessInboxMessagesConfiguration, ProcessInboxMessagesConfigurationValidator>(configuration, sectionName)
			.AddServiceOptions<IJobConfiguration, ProcessInboxMessagesJobConfiguration, ProcessInboxMessagesJobConfigurationValidator>(configuration, sectionName);
}
