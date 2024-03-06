namespace Modules.Hub.Application.Emails.GetEmails;

public sealed record GetEmailsQuery(ServiceAccountId AccountId, string ConversationId)
	: IAccountQuery<IReadOnlyList<Email>>;
