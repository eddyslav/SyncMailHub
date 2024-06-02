namespace Modules.Hub.Application.Emails;

public sealed record EmailsConversation(string Id
	, string? Subject
	, DateTimeOffset Date // last email date
	, EmailRecipient From);

