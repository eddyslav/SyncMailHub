namespace Modules.Hub.Application.Emails;

public sealed record Email(string Id
	, string? HtmlBody
	, DateTimeOffset Date // sent/received
	, EmailRecipient From
	, IReadOnlyList<EmailRecipient> To
	, IReadOnlyList<EmailRecipient> Cc
	, IReadOnlyList<EmailRecipient> Bcc);
