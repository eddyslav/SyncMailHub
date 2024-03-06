namespace Modules.Hub.Infrastucture.TokenService;

internal static class ServiceCollectionExtensions
{
	private static readonly string configurationSectionName = "JwtTokenService";

	public static IServiceCollection AddJwtTokenService(this IServiceCollection services, IConfiguration configuration) =>
		services
			.AddServiceOptions<JwtTokenServiceConfiguration, JwtTokenServiceConfigurationValidator>(configuration, configurationSectionName)
			.AddSingleton<ITokenService, JwtTokenService>();
}
