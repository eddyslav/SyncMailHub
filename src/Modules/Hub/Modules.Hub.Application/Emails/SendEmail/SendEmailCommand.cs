namespace Modules.Hub.Application.Emails.SendEmail;

public sealed record SendEmailCommand(ServiceAccountId AccountId
	, string Subject
	, string Body
	, IReadOnlyList<string> To
	, IReadOnlyList<string>? Cc
	, IReadOnlyList<string>? Bcc)
	: IAccountCommand<SendEmailResponse>;
