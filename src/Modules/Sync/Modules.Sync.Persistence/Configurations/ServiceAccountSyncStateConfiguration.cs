namespace Modules.Sync.Persistence.Configurations;

internal sealed class ServiceAccountSyncStateConfiguration : IEntityTypeConfiguration<ServiceAccountSyncState>
{
	public void Configure(EntityTypeBuilder<ServiceAccountSyncState> builder)
	{
		builder.ToTable("ServiceAccountSyncStates");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.ValueGeneratedNever()
			.HasConversion(x => x.Value, x => new ServiceAccountSyncStateId(x));

		builder.Property(x => x.AccountId)
			.HasConversion(x => x.Value, x => new ServiceAccountId(x));

		builder.HasOne<ServiceAccount>()
			.WithMany()
			.HasForeignKey(x => x.AccountId);
	}
}
