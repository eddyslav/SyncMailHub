namespace Modules.Hub.Application.Emails.EmailsConversations.GetEmailConversations;

internal sealed class GetEmailsConversationsQueryHandler(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: AccountQueryHandlerBase<GetEmailsConversationsQuery, PaginatedList<EmailsConversation>>(accountRepository, userContextAccessor, mailServiceFactory)
{
	protected override Task<Result<PaginatedList<EmailsConversation>>> HandleAsync(GetEmailsConversationsQuery query
		, IMailService mailService
		, CancellationToken cancellationToken) =>
		mailService.GetEmailsConversationsAsync(query.FolderId
			, query.PageToken
			, cancellationToken);
}
