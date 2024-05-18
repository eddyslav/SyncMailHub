namespace Modules.Hub.Infrastucture.Emails.MailService.Google;

internal static class ServiceCollectionExtensions
{
	private const string sectionName = "GoogleMailService";

	public static IServiceCollection AddGoogleMailService(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<MailServiceConfiguration, MailServiceConfigurationValidator>(configuration, sectionName);
}
