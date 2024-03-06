namespace Modules.Sync.Persistence.Configurations;

internal sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
	public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder) =>
		builder.ToTable("OutboxMessagesConsumers")
			.HasKey(x => new { x.Id, x.Name });
}
