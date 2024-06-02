namespace Modules.Sync.Application.ServiceAccounts;

internal sealed class ServiceAccountAddedIntegrationEventHandler(IServiceAccountRepository accountRepository, IUnitOfWork unitOfWork)
	: IntegrationEventHandler<ServiceAccountAddedIntegrationEvent>
{
	public override Task HandleAsync(ServiceAccountAddedIntegrationEvent integrationEvent, CancellationToken cancellationToken)
	{
		var serviceAccount = ServiceAccount.Create(integrationEvent.AccountId);

		accountRepository.Add(serviceAccount);
		return unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
