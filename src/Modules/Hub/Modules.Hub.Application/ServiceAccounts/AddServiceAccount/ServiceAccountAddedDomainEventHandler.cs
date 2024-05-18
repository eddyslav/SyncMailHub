namespace Modules.Hub.Application.ServiceAccounts.AddServiceAccount;

internal sealed class ServiceAccountAddedDomainEventHandler(IEventBus eventBus) : IDomainEventHandler<ServiceAccountAddedDomainEvent>
{
	public Task Handle(ServiceAccountAddedDomainEvent domainEvent, CancellationToken cancellationToken) =>
		eventBus.PublishAsync(new ServiceAccountAddedIntegrationEvent(domainEvent.Id
			, domainEvent.OccuredAt
			, domainEvent.AccountId.Value
			, domainEvent.AccountEmailAddress
			, domainEvent.UserId.Value
			, domainEvent.UserEmailAddress
			, domainEvent.UserFirstName
			, domainEvent.UserLastName)
		, cancellationToken);
}
