namespace Modules.Hub.Application.Emails;

public sealed record SendEmailRequest(string Subject
	, string Body
	, IReadOnlyList<string> To
	, IReadOnlyList<string>? Cc
	, IReadOnlyList<string>? Bcc);
