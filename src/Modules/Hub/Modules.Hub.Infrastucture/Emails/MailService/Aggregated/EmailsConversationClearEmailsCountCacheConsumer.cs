namespace Modules.Hub.Infrastucture.Emails.MailService.Aggregated;

internal sealed class EmailsConversationClearEmailsCountCacheConsumer(ICachingService cachingService
	, ILogger logger)
	: ServiceAccountStateChangeConsumerBase<EmailsConversationStateChange>
{
	private readonly ILogger logger = logger.ForContext<EmailsConversationClearEmailsCountCacheConsumer>();

	public override Task HandleAsync(ServiceAccountId accountId
		, EmailsConversationStateChange stateChange
		, CancellationToken cancellationToken)
	{
		var rawAccountId = accountId.Value;

		var cacheKeyToRemove = string.Format(CacheKeys.EmailsCountCacheKeyTemplate, rawAccountId);

		logger.Debug("Removing emails counter \"{cacheKey}\" cache key", cacheKeyToRemove);
		return cachingService.RemoveAsync(cacheKeyToRemove, cancellationToken);
	}
}
