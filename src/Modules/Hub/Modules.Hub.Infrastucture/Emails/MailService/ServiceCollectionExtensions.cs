using Modules.Hub.Infrastucture.Emails.MailService.Aggregated;
using Modules.Hub.Infrastucture.Emails.MailService.Google;

namespace Modules.Hub.Infrastucture.Emails.MailService;

internal static class ServiceCollectionExtensions
{
	public static IServiceCollection AddMailServices(this IServiceCollection services, IConfiguration configuration) =>
		services.AddAggregatedMailServices(configuration)
			.AddGoogleMailService(configuration)
			.AddScoped<IMailServiceFactory, MailServiceFactory>();
}
