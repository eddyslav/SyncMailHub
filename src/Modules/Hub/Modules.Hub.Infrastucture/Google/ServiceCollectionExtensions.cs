namespace Modules.Hub.Infrastucture.Google;

internal static class ServiceCollectionExtensions
{
	private static IServiceCollection AddGoogleOAuth(this IServiceCollection services, IConfiguration configuration)
	{
		const string sectionName = "Google:OAuth";

		return services.AddServiceOptions<GoogleOAuthConfiguration, GoogleOAuthConfigurationValidator>(configuration, sectionName); 
	}

	public static IServiceCollection AddGoogleCommonServices(this IServiceCollection services, IConfiguration configuration) =>
		services.AddGoogleOAuth(configuration);
}
