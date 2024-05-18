namespace Modules.Sync.Application.ServiceAccounts;

// remove all jobs here as well
internal sealed class ServiceAccountDeletedIntegrationEventHandler(IServiceAccountRepository accountRepository)
	: IntegrationEventHandler<ServiceAccountDeletedIntegrationEvent>
{
	public override Task Handle(ServiceAccountDeletedIntegrationEvent integrationEvent, CancellationToken cancellationToken) =>
		accountRepository.RemoveByHubIdAsync(integrationEvent.AccountId, cancellationToken);
}
