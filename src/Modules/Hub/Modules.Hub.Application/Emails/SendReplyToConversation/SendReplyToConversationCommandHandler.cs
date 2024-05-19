namespace Modules.Hub.Application.Emails.SendReplyToConversation;

internal sealed class SendReplyToConversationCommandHandler(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: AccountCommandHandlerBase<SendReplyToConversationCommand, SendReplyResponse>(accountRepository, userContextAccessor, mailServiceFactory)
{
	protected override Task<Result<SendReplyResponse>> HandleAsync(SendReplyToConversationCommand command
		, IMailService mailService
		, CancellationToken cancellationToken) =>
		mailService.SendReplyToConversationAsync(command.ConversationId
			, command.Body
			, cancellationToken);
}
