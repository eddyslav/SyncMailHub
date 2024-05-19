namespace Modules.Hub.Application.Emails.EmailsFolders.GetEmailsFolders;

internal sealed class GetEmailsFoldersQueryHandler(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: AccountQueryHandlerBase<GetEmailsFoldersQuery, IReadOnlyList<EmailsFolder>>(accountRepository, userContextAccessor, mailServiceFactory)
{
	protected override Task<Result<IReadOnlyList<EmailsFolder>>> HandleAsync(GetEmailsFoldersQuery _
		, IMailService mailService
		, CancellationToken cancellationToken) =>
		mailService.GetEmailsFoldersAsync(cancellationToken);
}
