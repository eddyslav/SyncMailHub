namespace Modules.Sync.Infrastructure.Emails.Google;

internal sealed class ServiceAccountSyncSessionConfiguration
{
	public required int MaxHistoriesToRetrievePerRequest { get; init; }
}
