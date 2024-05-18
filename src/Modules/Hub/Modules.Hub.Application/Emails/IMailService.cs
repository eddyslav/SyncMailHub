namespace Modules.Hub.Application.Emails;

public interface IMailService
{
	Task<Result<EmailsCounter>> GetEmailsCountAsync(CancellationToken cancellationToken);

	Task<Result<IReadOnlyList<EmailsFolder>>> GetEmailsFoldersAsync(CancellationToken cancellationToken = default);

	Task<Result<EmailsFolderCount>> GetEmailsFolderCountAsync(string folderId, CancellationToken cancellationToken = default);

	Task<Result<PaginatedList<EmailsConversation>>> GetEmailsConversationsAsync(string folderId
		, string? pageToken
		, CancellationToken cancellationToken = default);

	Task<Result<IReadOnlyList<Email>>> GetEmailsAsync(string conversationId, CancellationToken cancellationToken = default);
}
