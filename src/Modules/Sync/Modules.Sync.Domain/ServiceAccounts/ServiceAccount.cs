namespace Modules.Sync.Domain.ServiceAccounts;

public sealed class ServiceAccount : Entity<ServiceAccountId>, ICreatableEntity
{
	private ServiceAccount(ServiceAccountId id)
		: base(id)
	{
	}

	public Guid HubId { get; private init; }

	public DateTimeOffset CreatedAt { get; private init; }

	public static ServiceAccount Create(Guid hubId) =>
		new(new ServiceAccountId(Guid.NewGuid()))
		{
			HubId = hubId,
		};
}
