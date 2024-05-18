using Application.EventBus;

using Infrastructure.Caching;

using Infrastructure.DateTimeProvider;

using Infrastructure.EventBus;

using Infrastructure.Idempotence.Extensions;

using Modules.Hub.Application.Behaviors;

using Modules.Hub.Infrastucture.BackgroundJobs;
using Modules.Hub.Infrastucture.Emails.MailService;
using Modules.Hub.Infrastucture.EncryptionService;
using Modules.Hub.Infrastucture.Google;
using Modules.Hub.Infrastucture.PasswordHasherService;
using Modules.Hub.Infrastucture.Persistence;
using Modules.Hub.Infrastucture.ServiceAccountCredentialsProvider;
using Modules.Hub.Infrastucture.ServiceAccounts.AddServiceAccount.Google;
using Modules.Hub.Infrastucture.TokenService;
using Modules.Hub.Infrastucture.UserContext;

namespace Modules.Hub.Infrastucture;

public static class ServiceCollectionExtensions
{
	private const string configurationSectionName = "Modules:Hub";

	public static IServiceCollection AddHubModule(this IServiceCollection services, IConfiguration configuration)
	{
		configuration = configuration.GetRequiredSection(configurationSectionName);

		return services.AddHttpContextAccessor()
			.AddMediatR(config =>
				config.TapAction(config => config.Lifetime = ServiceLifetime.Scoped)
					.RegisterServicesFromAssemblies(Application.AssemblyMarker.Assembly, AssemblyMarker.Assembly)
					.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>))
					.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>)))
			.AddValidatorsFromAssemblies([Application.AssemblyMarker.Assembly, AssemblyMarker.Assembly]
				, ServiceLifetime.Singleton
				, includeInternalTypes: true)
			.DecorateDomainEventHandlersWithIdempotency<HubDbContext>(Application.AssemblyMarker.Assembly)
			.AddBackgroundJobs(configuration)
			.AddMailServices(configuration)
			.AddEncryptionService(configuration)
			.AddGoogleCommonServices(configuration)
			.AddPasswordHasher(configuration)
			.AddPersistence(configuration)
			.AddServiceAccountCredentialsProvider(configuration)
			.AddJwtTokenService(configuration)
			.AddScoped<IUserContextAccessor, UserContextAccessor>()
			.AddSingleton<GoogleServiceAccountCredentialsVerifier>()
			.TapAction(services.TryAddTransient<IEventBus, EventBus>)
			.TapAction(services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>);
	}
}
