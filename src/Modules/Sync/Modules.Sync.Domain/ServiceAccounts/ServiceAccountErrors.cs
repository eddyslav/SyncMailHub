using Shared.Results;

namespace Modules.Sync.Domain.ServiceAccounts;

public static class ServiceAccountErrors
{
	public static Error NotFound { get; } = new("ServiceAccount.NotFound", "The service account does not exist");
	public static Error InvalidCredentials { get; } = new("ServiceAccount.InvalidCredentials", "The service account's credentials were invalid");

	public static class Google
	{
		public static Error ApiRequestFailed { get; } = new("GoogleServiceAccount.ApiRequestFailed", "Failed to retrieve mailbox data with unknown external error");
	}
}
