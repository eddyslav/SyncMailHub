namespace Modules.Sync.Persistence.Configurations;

internal sealed class GoogleServiceAccountSyncStateConfiguration : IEntityTypeConfiguration<GoogleServiceAccountSyncState>
{
	public void Configure(EntityTypeBuilder<GoogleServiceAccountSyncState> builder)
	{
		builder.ToTable("GoogleServiceAccountSyncStates");
	}
}
