namespace Modules.Hub.Infrastucture.BackgroundJobs.ProcessOutboxMessages;

internal static class ProcessOutboxMessagesServiceCollectionExtensions
{
	private static readonly string sectionName = "ProcessOutboxMessages";

	public static IServiceCollection AddProcessOutboxMessagesJob(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<ProcessOutboxMessagesConfiguration, ProcessOutboxMessagesConfigurationValidator>(configuration, sectionName)
			.AddServiceOptions<IJobConfiguration, ProcessOutboxMessagesJobConfiguration, ProcessOutboxMessagesJobConfigurationValidator>(configuration, sectionName);
}
