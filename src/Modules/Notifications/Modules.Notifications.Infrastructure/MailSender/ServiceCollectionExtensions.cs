using Modules.Notifications.Application.MailSender;

namespace Modules.Notifications.Infrastructure.MailSender;

internal static class ServiceCollectionExtensions
{
	private static readonly string configurationSectionName = "MailSender";

	public static IServiceCollection AddMailSender(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<MailSenderConfiguration, MailSenderConfigurationValidator>(configuration, configurationSectionName)
			.AddSingleton<IMailSender, MailSender>();
}
