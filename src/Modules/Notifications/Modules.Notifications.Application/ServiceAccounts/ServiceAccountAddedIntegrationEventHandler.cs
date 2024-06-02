namespace Modules.Notifications.Application.ServiceAccounts;

internal sealed class ServiceAccountAddedIntegrationEventHandler(IMailSender emailSender) : IntegrationEventHandler<ServiceAccountAddedIntegrationEvent>
{
	public override Task HandleAsync(ServiceAccountAddedIntegrationEvent integrationEvent, CancellationToken cancellationToken) =>
		emailSender.SendAccountConnectedAsync(new SendAccountConnectedRequest(integrationEvent.AccountEmailAddress, integrationEvent.UserFirstName), cancellationToken);
}
