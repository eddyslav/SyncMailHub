namespace Modules.Hub.Application.Emails.EmailsConversations.DeleteEmailsConversation;

public sealed record DeleteEmailsConversationCommand(ServiceAccountId AccountId
	, string ConversationId
	, bool Force)
	: IAccountCommand;
