namespace Modules.Sync.Domain.ServiceAccountSyncStates;

public abstract class ServiceAccountSyncState(ServiceAccountSyncStateId Id) : Entity<ServiceAccountSyncStateId>(Id)
{
	public ServiceAccountId AccountId { get; protected init; } = default!;
}
