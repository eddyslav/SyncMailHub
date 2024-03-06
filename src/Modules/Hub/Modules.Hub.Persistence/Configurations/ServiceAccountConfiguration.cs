namespace Modules.Hub.Persistence.Configurations;

internal sealed class ServiceAccountConfiguration : IEntityTypeConfiguration<ServiceAccount>
{
	public void Configure(EntityTypeBuilder<ServiceAccount> builder)
	{
		builder.ToTable("ServiceAccounts");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.ValueGeneratedNever()
			.HasConversion(x => x.Value, x => new ServiceAccountId(x));

		builder.Property(x => x.UserId)
			.HasConversion(x => x.Value, x => new UserId(x));

		builder.Property(x => x.ExternalId)
			.HasMaxLength(36);

		builder.HasOne<User>()
			.WithMany()
			.HasForeignKey(x => x.UserId);

		builder.HasIndex(x => x.ExternalId)
			.IsUnique();

		builder.HasIndex(x => x.EmailAddress)
			.IsUnique();
	}
}
