namespace Modules.Sync.Domain.ServiceAccountSyncStates;

public sealed class GoogleServiceAccountSyncState : ServiceAccountSyncState
{
	private GoogleServiceAccountSyncState(ServiceAccountSyncStateId id)
		: base(id)
	{
	}

	public ulong HistoryId { get; private set; }

	public static GoogleServiceAccountSyncState Create(ServiceAccountId accountId, ulong historyId) =>
		new(new ServiceAccountSyncStateId(Guid.NewGuid()))
		{
			AccountId = accountId,
			HistoryId = historyId
		};

	public void UpdateSyncState(ulong historyId) => HistoryId = historyId;
}
