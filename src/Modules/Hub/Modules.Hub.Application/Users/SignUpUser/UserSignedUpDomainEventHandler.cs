using Modules.Hub.Domain.Users.DomainEvents;

namespace Modules.Hub.Application.Users.SignUpUser;

internal sealed class UserSignedUpDomainEventHandler(IEventBus eventBus) : IDomainEventHandler<UserSignedUpDomainEvent>
{
	public Task Handle(UserSignedUpDomainEvent domainEvent, CancellationToken cancellationToken) =>
		eventBus.PublishAsync(new UserSignedUpIntegrationEvent(domainEvent.Id
			, domainEvent.OccuredAt
			, domainEvent.UserId.Value
			, domainEvent.EmailAddress
			, domainEvent.FirstName
			, domainEvent.LastName)
		, cancellationToken);
}
