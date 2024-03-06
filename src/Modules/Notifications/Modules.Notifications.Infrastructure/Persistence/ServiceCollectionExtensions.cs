namespace Modules.Notifications.Infrastructure.Persistence;

internal static class ServiceCollectionExtensions
{
	private static readonly string connectionStringSettingName = "NotificationsDb";

	public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) =>
		services.AddDbContext<NotificationsDbContext>((serviceProvider, options) =>
			options.UseNpgsql(configuration.GetConnectionString(connectionStringSettingName)
				, optionsBuilder => optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", SchemaNames.Notifications)));
}
