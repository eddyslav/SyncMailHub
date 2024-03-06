namespace Modules.Sync.Persistence;

public sealed class SyncDbContext(DbContextOptions<SyncDbContext> options) : DbContext(options)
{
	public required DbSet<ServiceAccount> ServiceAccounts { get; init; }

	public required DbSet<ServiceAccountSyncState> SyncStates { get; init; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) =>
		modelBuilder.HasDefaultSchema(SchemaNames.Sync)
			.ApplyConfigurationsFromAssembly(AssemblyMarker.Assembly);
}
