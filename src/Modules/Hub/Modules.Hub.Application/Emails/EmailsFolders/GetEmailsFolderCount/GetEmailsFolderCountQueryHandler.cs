namespace Modules.Hub.Application.Emails.EmailsFolders.GetEmailsFolderCount;

internal sealed class GetEmailsFolderCountQueryHandler(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: AccountQueryHandlerBase<GetEmailsFolderCountQuery, EmailsFolderCount>(accountRepository, userContextAccessor, mailServiceFactory)
{
	protected override Task<Result<EmailsFolderCount>> HandleAsync(GetEmailsFolderCountQuery query
		, IMailService mailService
		, CancellationToken cancellationToken) =>
		mailService.GetEmailsFolderCountAsync(query.FolderId, cancellationToken);
}
