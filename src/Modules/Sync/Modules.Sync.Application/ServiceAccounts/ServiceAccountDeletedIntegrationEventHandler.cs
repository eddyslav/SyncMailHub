namespace Modules.Sync.Application.ServiceAccounts;

// remove all jobs here as well
internal sealed class ServiceAccountDeletedIntegrationEventHandler(IServiceAccountRepository accountRepository)
	: IntegrationEventHandler<ServiceAccountDeletedIntegrationEvent>
{
	public override Task HandleAsync(ServiceAccountDeletedIntegrationEvent integrationEvent, CancellationToken cancellationToken) =>
		accountRepository.RemoveByHubIdAsync(integrationEvent.AccountId, cancellationToken);
}
