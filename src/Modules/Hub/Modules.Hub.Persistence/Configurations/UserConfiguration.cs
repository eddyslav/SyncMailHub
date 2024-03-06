namespace Modules.Hub.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("Users");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.ValueGeneratedNever()
			.HasConversion(x => x.Value, x => new UserId(x));

		builder.Property(x => x.EmailAddress)
			.HasMaxLength(UserConstants.EmailAddressMaxLength);

		builder.Property(x => x.FirstName)
			.HasMaxLength(UserConstants.FirstNameMaxLength);

		builder.Property(x => x.LastName)
			.HasMaxLength(UserConstants.LastNameMaxLength);

		builder.HasIndex(x => x.EmailAddress)
			.IsUnique();
	}
}
