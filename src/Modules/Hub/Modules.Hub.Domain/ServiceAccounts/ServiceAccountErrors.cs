namespace Modules.Hub.Domain.ServiceAccounts;

public static class ServiceAccountErrors
{
	public static Error NotFound { get; } = new NotFoundError("ServiceAccount.NotFound", "The service account does not exist");
	public static Error InvalidCredentials { get; } = new BadRequestError("ServiceAccount.InvalidCredentials", "The service account's credentials were invalid");
	public static Error AlreadyExists { get; } = new ConflictError("ServiceAccount.AlreadyAdded", "The service account was already added");
	public static Error UserNotFound { get; } = new NotFoundError("ServiceAccount.UserNotFound", "The target user does not exist");

	public static class Google
	{
		public static Error StateMismatch { get; } = new BadRequestError("GoogleServiceAccount.MismatchState", "State has been changed. Try authenticating again");
		public static Error TokenError { get; } = new BadRequestError("GoogleServiceAccount.AuthFailed", "Retrieved token does not contain completed information. Please try setting account again");
		public static Error ApiRequestFailed { get; } = new("GoogleServiceAccount.ApiRequestFailed", "Failed to retrieve mailbox data with unknown external error");
	}
}
