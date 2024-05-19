namespace Modules.Hub.Application.Emails.EmailsConversations.DeleteEmailsConversation;

internal sealed class DeleteEmailsConversationCommandHandler(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: AccountCommandHandlerBase<DeleteEmailsConversationCommand>(accountRepository, userContextAccessor, mailServiceFactory)
{
	protected override Task<Result> HandleAsync(DeleteEmailsConversationCommand command
		, IMailService mailService
		, CancellationToken cancellationToken) =>
		command.Force
			? mailService.DeleteConversationByIdAsync(command.ConversationId, cancellationToken)
			: mailService.TrashConversationByIdAsync(command.ConversationId, cancellationToken);
}
