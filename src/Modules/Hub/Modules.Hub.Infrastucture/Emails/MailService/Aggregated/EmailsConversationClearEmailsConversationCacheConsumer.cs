namespace Modules.Hub.Infrastucture.Emails.MailService.Aggregated;

internal sealed class EmailsConversationClearEmailsConversationCacheConsumer(ICachingService cachingService
	, ILogger logger)
	: ServiceAccountStateChangeConsumerBase<EmailsConversationStateChange>
{
	private readonly ILogger logger = logger.ForContext<EmailsConversationClearEmailsConversationCacheConsumer>();

	public override async Task HandleAsync(ServiceAccountId accountId
		, EmailsConversationStateChange stateChange
		, CancellationToken cancellationToken)
	{
		var rawAccountId = accountId.Value;

		var folderIds = stateChange.FolderIds;
		foreach (var folderId in folderIds)
		{
			var cacheKeyToRemove = string.Format(CacheKeys.EmailsConversationsCacheKeyPrefixTemplate, rawAccountId, folderId);
			
			logger.Debug("Removing emails conversation \"{cacheKey}\" cache key", cacheKeyToRemove);
			await cachingService.RemoveByPrefixAsync(cacheKeyToRemove, cancellationToken);
		}
	}
}
