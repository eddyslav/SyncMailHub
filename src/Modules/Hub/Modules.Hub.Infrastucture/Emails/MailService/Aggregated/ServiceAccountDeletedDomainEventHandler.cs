using Modules.Hub.Domain.ServiceAccounts.DomainEvents;

namespace Modules.Hub.Infrastucture.Emails.MailService.Aggregated;

internal sealed class ServiceAccountDeletedDomainEventHandler(ICachingService cachingService
	, ILogger logger)
	: IDomainEventHandler<ServiceAccountDeletedDomainEvent>
{
	private readonly ILogger logger = logger.ForContext<ServiceAccountDeletedDomainEventHandler>();

	public Task Handle(ServiceAccountDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
	{
		logger.Debug("Removing all cache keys related to the \"{accountEmailAddress}\" account", domainEvent.EmailAddress);

		var cacheKeyPrefix = string.Format(CacheKeys.AccountCredentialsCacheKeyTemplate, domainEvent.AccountId.Value);
		return cachingService.RemoveByPrefixAsync(cacheKeyPrefix, cancellationToken);
	}
}
