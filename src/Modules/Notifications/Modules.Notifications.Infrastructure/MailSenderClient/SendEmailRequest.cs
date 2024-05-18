namespace Modules.Notifications.Infrastructure.EmailSenderClient;

internal sealed record SendEmailRequest(string Subject
	, string Body
	, IReadOnlyCollection<string> To
	, IReadOnlyCollection<string>? Cc = null);
