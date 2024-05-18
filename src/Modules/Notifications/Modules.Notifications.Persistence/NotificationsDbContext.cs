namespace Modules.Notifications.Persistence;

public sealed class NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder) =>
		modelBuilder.HasDefaultSchema(SchemaNames.Notifications)
			.ApplyConfigurationsFromAssembly(AssemblyMarker.Assembly);
}
