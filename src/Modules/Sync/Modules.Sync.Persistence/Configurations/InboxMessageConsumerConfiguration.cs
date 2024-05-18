namespace Modules.Sync.Persistence.Configurations;

internal sealed class InboxMessageConsumerConfiguration : IEntityTypeConfiguration<InboxMessageConsumer>
{
	public void Configure(EntityTypeBuilder<InboxMessageConsumer> builder) =>
		builder.ToTable("InboxMessagesConsumers")
			.HasKey(x => new { x.Id, x.Name });
}
