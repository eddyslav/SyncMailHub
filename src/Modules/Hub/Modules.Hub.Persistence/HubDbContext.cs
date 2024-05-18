namespace Modules.Hub.Persistence;

public sealed class HubDbContext(DbContextOptions<HubDbContext> options) : DbContext(options)
{
	public required DbSet<User> Users { get; init; }

	public required DbSet<ServiceAccount> ServiceAccounts { get; init; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder) =>
		modelBuilder.HasDefaultSchema(SchemaNames.Hub)
			.ApplyConfigurationsFromAssembly(AssemblyMarker.Assembly);
}
