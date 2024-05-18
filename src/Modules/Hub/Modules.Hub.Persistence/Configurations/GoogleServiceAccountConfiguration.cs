namespace Modules.Hub.Persistence.Configurations;

internal sealed class GoogleServiceAccountConfiguration : IEntityTypeConfiguration<GoogleServiceAccount>
{
	public void Configure(EntityTypeBuilder<GoogleServiceAccount> builder) =>
		builder.ToTable("GoogleServiceAccounts");
}
