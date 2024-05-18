namespace Modules.Hub.Application.Emails.EmailsConversations.GetEmailConversations;

public sealed record GetEmailsConversationsQuery(ServiceAccountId AccountId
	, string FolderId
	, string? PageToken)
	: IAccountQuery<PaginatedList<EmailsConversation>>;
