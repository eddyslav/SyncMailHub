namespace Modules.Hub.Infrastucture.PasswordHasherService;

internal static class ServiceCollectionExtensions
{
	private static readonly string sectionName = "PasswordHasher";

	public static IServiceCollection AddPasswordHasher(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<BCryptPasswordHasherServiceConfiguration, BCryptPasswordHasherServiceConfigurationValidator>(configuration, sectionName)
			.AddSingleton<IPasswordHasherService, BCryptPasswordHasherService>();
}
