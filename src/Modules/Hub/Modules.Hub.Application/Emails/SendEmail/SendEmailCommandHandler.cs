namespace Modules.Hub.Application.Emails.SendEmail;

internal sealed class SendEmailCommandHandler(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: AccountCommandHandlerBase<SendEmailCommand, SendEmailResponse>(accountRepository, userContextAccessor, mailServiceFactory)
{
	protected override Task<Result<SendEmailResponse>> HandleAsync(SendEmailCommand command
		, IMailService mailService
		, CancellationToken cancellationToken) =>
		Result.Create(command)
			.Map(command =>
				new SendEmailRequest(command.Subject
					, command.Body
					, command.To
					, command.Cc
					, command.Bcc))
			.Bind(request => mailService.SendEmailAsync(request, cancellationToken));
}
