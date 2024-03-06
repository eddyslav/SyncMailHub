namespace Modules.Hub.Infrastucture.EncryptionService;

internal static class ServiceCollectionExtensions
{
	private static readonly string sectionName = "EncryptionService";

	public static IServiceCollection AddEncryptionService(this IServiceCollection services, IConfiguration configuration) =>
		services.AddServiceOptions<AesEncryptionServiceConfiguration, AesEncryptionServiceConfigurationValidator>(configuration, sectionName)
			.AddSingleton<IEncryptionService, AesEncryptionService>();
}
