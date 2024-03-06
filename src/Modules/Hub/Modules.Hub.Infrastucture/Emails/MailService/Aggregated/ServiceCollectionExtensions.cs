namespace Modules.Hub.Infrastucture.Emails.MailService.Aggregated;

internal static class ServiceCollectionExtensions
{
	private const string sectionName = "AggregatedMailService";

	public static IServiceCollection AddAggregatedMailServices(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<MailServiceConfiguration, MailServiceConfigurationValidator>(configuration, sectionName)
			.AddScoped<EmailsConversationClearEmailsCacheConsumer>()
			.AddScoped<EmailsConversationClearEmailsConversationCacheConsumer>()
			.AddScoped<EmailsConversationClearEmailsCountCacheConsumer>()
			.AddScoped<EmailsConversationClearEmailsFolderCountCacheConsumer>();
}
