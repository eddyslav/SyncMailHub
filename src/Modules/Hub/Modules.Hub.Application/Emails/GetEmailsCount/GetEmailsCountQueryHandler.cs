namespace Modules.Hub.Application.Emails.GetEmailsCount;

internal sealed class GetEmailsCountQueryHandler(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: AccountQueryHandlerBase<GetEmailsCountQuery, EmailsCounter>(accountRepository, userContextAccessor, mailServiceFactory)
{
	protected override Task<Result<EmailsCounter>> HandleAsync(GetEmailsCountQuery query
		, IMailService mailService
		, CancellationToken cancellationToken) =>
		mailService.GetEmailsCountAsync(cancellationToken);
}
