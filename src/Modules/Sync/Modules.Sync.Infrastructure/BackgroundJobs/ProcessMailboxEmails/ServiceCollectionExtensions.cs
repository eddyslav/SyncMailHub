namespace Modules.Sync.Infrastructure.BackgroundJobs.ProcessMailboxEmails;

internal static class ServiceCollectionExtensions
{
	private static readonly string sectionName = "ProcessMailboxEmails";

	public static IServiceCollection AddProcessMailboxEmailsJob(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<IJobConfiguration, ProcessMailboxEmailsJobConfiguration, ProcessMailboxEmailsJobConfigurationValidator>(configuration, sectionName);
}
