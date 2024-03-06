namespace Modules.Hub.Application.Emails;

public static class EmailsConversationErrors
{
	public static Error NotFound { get; } = new NotFoundError("EmailsConversation.NotFound", "The email conversation does not exist");
}
