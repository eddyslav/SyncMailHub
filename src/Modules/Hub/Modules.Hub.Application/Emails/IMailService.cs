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

	Task<Result<SendEmailResponse>> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken = default);

	Task<Result<SendReplyResponse>> SendReplyToConversationAsync(string conversationId
		, string body
		, CancellationToken cancellationToken = default);

	Task<Result> TrashEmailByIdAsync(string id, CancellationToken cancellationToken = default);

	Task<Result> DeleteEmailByIdAsync(string id, CancellationToken cancellationToken = default);

	Task<Result> TrashConversationByIdAsync(string id, CancellationToken cancellationToken = default);

	Task<Result> DeleteConversationByIdAsync(string id, CancellationToken cancellationToken = default);
}
