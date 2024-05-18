namespace Modules.Hub.Application.Emails;

public static class EmailsFolderErrors
{
	public static Error NotFound { get; } = new NotFoundError("EmailsFolder.NotFound", "The emails folder does not exist");
}
