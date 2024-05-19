namespace Modules.Hub.Application.Emails.SendReplyToConversation;

public sealed record SendReplyToConversationCommand(ServiceAccountId AccountId
	, string ConversationId
	, string Body)
	: IAccountCommand<SendReplyResponse>;
