using Microsoft.EntityFrameworkCore;

using Modules.Hub.Persistence;

using Modules.Notifications.Persistence;

using Modules.Sync.Persistence;

namespace App.Extensions;

internal static class WebApplicationExtensions
{
	private static void MigrateDatabase<TDbContext>(IServiceScope serviceScope)
		where TDbContext : DbContext
	{
		var dbContext = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
		dbContext.Database.Migrate();
	}

	public static WebApplication UseDatabaseInitializer(this WebApplication webApplication)
	{
		if (!webApplication.Environment.IsDevelopment())
		{
			return webApplication;
		}

		using (var serviceScope = webApplication.Services.CreateScope())
		{
			MigrateDatabase<HubDbContext>(serviceScope);
			MigrateDatabase<NotificationsDbContext>(serviceScope);
			MigrateDatabase<SyncDbContext>(serviceScope);
		}

		return webApplication;
	}
}
