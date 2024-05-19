namespace Modules.Hub.Application.Emails.DeleteEmail;

internal sealed class DeleteEmailCommandHandlerHandler(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: AccountCommandHandlerBase<DeleteEmailCommand>(accountRepository, userContextAccessor, mailServiceFactory)
{
	protected override Task<Result> HandleAsync(DeleteEmailCommand command
		, IMailService mailService
		, CancellationToken cancellationToken) =>
		command.Force
			? mailService.DeleteEmailByIdAsync(command.EmailId, cancellationToken)
			: mailService.TrashEmailByIdAsync(command.EmailId, cancellationToken);
}
