namespace Modules.Hub.Application.Emails;

public static class EmailErrors
{
	public static Error NotFound { get; } = new NotFoundError("Email.NotFound", "The email does not exist");
}
