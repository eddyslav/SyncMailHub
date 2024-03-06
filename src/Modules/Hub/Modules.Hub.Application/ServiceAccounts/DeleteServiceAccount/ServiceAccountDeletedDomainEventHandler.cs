namespace Modules.Hub.Application.ServiceAccounts.AddServiceAccount;

internal sealed class ServiceAccountDeletedDomainEventHandler(IEventBus eventBus) : IDomainEventHandler<ServiceAccountDeletedDomainEvent>
{
	public Task Handle(ServiceAccountDeletedDomainEvent domainEvent, CancellationToken cancellationToken) =>
		eventBus.PublishAsync(new ServiceAccountDeletedIntegrationEvent(domainEvent.Id
			, domainEvent.OccuredAt
			, domainEvent.AccountId.Value
			, domainEvent.EmailAddress)
		, cancellationToken);
}
