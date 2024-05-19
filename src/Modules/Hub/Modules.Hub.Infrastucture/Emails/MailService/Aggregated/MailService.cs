namespace Modules.Hub.Infrastucture.Emails.MailService.Aggregated;

internal sealed class MailService(IMailService mailService
	, ICachingService cachingService
	, IServiceAccountContextAccessor accountContextAccessor
	, IOptions<MailServiceConfiguration> options) : IMailService
{
	private readonly MailServiceConfiguration configuration = options.GetConfiguration();

	public Task<Result<EmailsCounter>> GetEmailsCountAsync(CancellationToken cancellationToken = default)
	{
		Task<Result<EmailsCounter>> AsyncValueFactory(CacheEntryOptions entryOptions)
		{
			entryOptions.SetAbsoluteExpirationRelativeToNow(configuration.EmailsCountCacheLifetime);

			return mailService.GetEmailsCountAsync(cancellationToken);
		}

		var accountId = accountContextAccessor.AccountId;

		var cacheKey = string.Format(CacheKeys.EmailsCountCacheKeyTemplate, accountId.Value);
		return cachingService.GetOrAddAsync(cacheKey
			, AsyncValueFactory
			, cancellationToken);
	}

	public Task<Result<IReadOnlyList<EmailsFolder>>> GetEmailsFoldersAsync(CancellationToken cancellationToken = default)
	{
		Task<Result<IReadOnlyList<EmailsFolder>>> AsyncValueFactory(CacheEntryOptions entryOptions)
		{
			entryOptions.SetAbsoluteExpirationRelativeToNow(configuration.EmailsFoldersCacheLifetime);

			return mailService.GetEmailsFoldersAsync(cancellationToken);
		}

		var accountId = accountContextAccessor.AccountId;

		var cacheKey = string.Format(CacheKeys.EmailsFoldersCacheKeyTemplate, accountId.Value);
		return cachingService.GetOrAddAsync(cacheKey
			, AsyncValueFactory
			, cancellationToken);
	}

	public Task<Result<EmailsFolderCount>> GetEmailsFolderCountAsync(string folderId, CancellationToken cancellationToken = default)
	{
		Task<Result<EmailsFolderCount>> AsyncValueFactory(CacheEntryOptions entryOptions)
		{
			entryOptions.SetAbsoluteExpirationRelativeToNow(configuration.EmailsFolderCountCacheLifetime);

			return mailService.GetEmailsFolderCountAsync(folderId, cancellationToken);
		}

		var accountId = accountContextAccessor.AccountId;

		var cacheKey = string.Format(CacheKeys.EmailsFolderCountCacheKeyTemplate
			, accountId.Value
			, folderId);

		return cachingService.GetOrAddAsync(cacheKey
			, AsyncValueFactory
			, cancellationToken);
	}

	public Task<Result<PaginatedList<EmailsConversation>>> GetEmailsConversationsAsync(string folderId
		, string? nextPageToken
		, CancellationToken cancellationToken = default)
	{
		Task<Result<PaginatedList<EmailsConversation>>> AsyncValueFactory(CacheEntryOptions entryOptions)
		{
			entryOptions.SetAbsoluteExpirationRelativeToNow(configuration.EmailsConversationsCacheLifetime);

			return mailService.GetEmailsConversationsAsync(folderId
				, nextPageToken
				, cancellationToken);
		}

		var accountId = accountContextAccessor.AccountId;

		var cacheKey = string.Format(CacheKeys.EmailsConversationsCacheKeyTemplate
			, accountId.Value
			, folderId
			, nextPageToken);

		return cachingService.GetOrAddAsync(cacheKey
			, AsyncValueFactory
			, cancellationToken);
	}

	public Task<Result<IReadOnlyList<Email>>> GetEmailsAsync(string conversationId, CancellationToken cancellationToken = default)
	{
		Task<Result<IReadOnlyList<Email>>> AsyncValueFactory(CacheEntryOptions entryOptions)
		{
			entryOptions.SetAbsoluteExpirationRelativeToNow(configuration.EmailsCacheLifeTime);

			return mailService.GetEmailsAsync(conversationId, cancellationToken);
		}

		var accountId = accountContextAccessor.AccountId;

		var cacheKey = string.Format(CacheKeys.EmailsCacheKeyTemplate
			, accountId.Value
			, conversationId);

		return cachingService.GetOrAddAsync(cacheKey
			, AsyncValueFactory
			, cancellationToken);
	}

	public Task<Result<SendEmailResponse>> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken = default) =>
		mailService.SendEmailAsync(request, cancellationToken);

	public Task<Result<SendReplyResponse>> SendReplyToConversationAsync(string conversationId, string body, CancellationToken cancellationToken = default) =>
		mailService.SendReplyToConversationAsync(conversationId, body, cancellationToken);

	public Task<Result> TrashEmailByIdAsync(string id, CancellationToken cancellationToken = default) =>
		mailService.TrashEmailByIdAsync(id, cancellationToken);

	public Task<Result> DeleteEmailByIdAsync(string id, CancellationToken cancellationToken = default) =>
		mailService.DeleteEmailByIdAsync(id, cancellationToken);

	public Task<Result> TrashConversationByIdAsync(string id, CancellationToken cancellationToken = default) =>
		mailService.TrashConversationByIdAsync(id, cancellationToken);

	public Task<Result> DeleteConversationByIdAsync(string id, CancellationToken cancellationToken = default) =>
		mailService.DeleteConversationByIdAsync(id, cancellationToken);
}
