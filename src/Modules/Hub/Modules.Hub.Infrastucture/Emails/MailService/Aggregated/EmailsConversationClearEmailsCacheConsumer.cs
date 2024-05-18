namespace Modules.Hub.Infrastucture.Emails.MailService.Aggregated;

internal sealed class EmailsConversationClearEmailsCacheConsumer(ICachingService cachingService
	, ILogger logger)
	: ServiceAccountStateChangeConsumerBase<EmailsConversationStateChange>
{
	private readonly ILogger logger = logger.ForContext<EmailsConversationClearEmailsCacheConsumer>();

	public override Task HandleAsync(ServiceAccountId accountId
		, EmailsConversationStateChange stateChange
		, CancellationToken cancellationToken)
	{
		var rawAccountId = accountId.Value;
		var cacheKeyToRemove = string.Format(CacheKeys.EmailsCacheKeyTemplate, rawAccountId, stateChange.Id);

		logger.Debug("Removing emails conversations \"{cacheKey}\" cache key prefix", cacheKeyToRemove);
		return cachingService.RemoveAsync(cacheKeyToRemove, cancellationToken);
	}
}
