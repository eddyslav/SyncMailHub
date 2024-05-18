namespace Modules.Hub.Application.Emails.GetEmails;

internal sealed class GetEmailsQueryHandler(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: AccountQueryHandlerBase<GetEmailsQuery, IReadOnlyList<Email>>(accountRepository, userContextAccessor, mailServiceFactory)
{
	protected override Task<Result<IReadOnlyList<Email>>> HandleAsync(GetEmailsQuery query
		, IMailService mailService
		, CancellationToken cancellationToken) =>
		mailService.GetEmailsAsync(query.ConversationId, cancellationToken);
}
