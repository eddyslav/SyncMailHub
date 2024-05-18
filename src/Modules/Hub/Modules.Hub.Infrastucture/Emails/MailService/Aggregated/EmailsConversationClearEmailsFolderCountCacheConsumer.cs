namespace Modules.Hub.Infrastucture.Emails.MailService.Aggregated;

internal sealed class EmailsConversationClearEmailsFolderCountCacheConsumer(ICachingService cachingService
	, ILogger logger)
	: ServiceAccountStateChangeConsumerBase<EmailsConversationStateChange>
{
	private readonly ILogger logger = logger.ForContext<EmailsConversationClearEmailsFolderCountCacheConsumer>();

	public override async Task HandleAsync(ServiceAccountId accountId
		, EmailsConversationStateChange stateChange
		, CancellationToken cancellationToken)
	{
		var rawAccountId = accountId.Value;

		var folderIds = stateChange.FolderIds;
		foreach (var folderId in folderIds)
		{
			var cacheKeyToRemove = string.Format(CacheKeys.EmailsFolderCountCacheKeyTemplate, rawAccountId, folderId);
			
			logger.Debug("Removing emails folder counter \"{cacheKey}\" cache key", cacheKeyToRemove);
			await cachingService.RemoveAsync(cacheKeyToRemove, cancellationToken);
		}
	}
}
