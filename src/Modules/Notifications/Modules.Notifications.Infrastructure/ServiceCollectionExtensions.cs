using Infrastructure.DateTimeProvider;

using Modules.Notifications.Infrastructure.BackgroundJobs;
using Modules.Notifications.Infrastructure.MailSender;
using Modules.Notifications.Infrastructure.Persistence;

namespace Modules.Notifications.Infrastructure;

public static class ServiceCollectionExtensions
{
	private const string configurationSectionName = "Modules:Notifications";

	public static IServiceCollection AddNotificationsModule(this IServiceCollection services, IConfiguration configuration)
	{
		configuration = configuration.GetRequiredSection(configurationSectionName);

		return services.DecorateIntegrationEventHandlersWithIdempotency<NotificationsDbContext>(Application.AssemblyMarker.Assembly)
			.AddBackgroundJobs(configuration)
			.AddMailSenderClient(configuration)
			.AddMailSender(configuration)
			.AddPersistence(configuration)
			.AddSingleton<ITemplateRendererService, FluidTemplateRendererService>()
			.TapAction(services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>);
	}
}
