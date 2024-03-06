namespace Modules.Notifications.Infrastructure.EmailSenderClient;

internal static class ServiceCollectionExtensions
{
	private static readonly string configurationSectionName = "MailSenderClient";

	public static IServiceCollection AddMailSenderClient(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<SmtpMailSenderClientConfiguration, SmtpMailSenderClientConfigurationValidator>(configuration, configurationSectionName)
			.AddSingleton<IMailSenderClient, SmtpMailSenderClient>();
}
