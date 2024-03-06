namespace Modules.Sync.Persistence.Configurations;

internal sealed class ServiceAccountConfiguration : IEntityTypeConfiguration<ServiceAccount>
{
	public void Configure(EntityTypeBuilder<ServiceAccount> builder)
	{
		builder.ToTable("ServiceAccounts");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.ValueGeneratedNever()
			.HasConversion(x => x.Value, x => new ServiceAccountId(x));

		builder.HasIndex(x => x.HubId)
			.IsUnique();
	}
}
