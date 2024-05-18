namespace Modules.Sync.Application.Emails;

public static class Errors
{
	public static Error InvalidCredentials { get; } = new("ServiceAccount.InvalidCredentials", "Service account credentials are invalid");
	
}
