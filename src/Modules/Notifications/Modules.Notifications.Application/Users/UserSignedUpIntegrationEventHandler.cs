using Modules.Notifications.Application.MailSender;

namespace Modules.Notifications.Application.Users;

internal sealed class UserSignedUpIntegrationEventHandler(IMailSender emailSender) : IntegrationEventHandler<UserSignedUpIntegrationEvent>
{
	public override Task Handle(UserSignedUpIntegrationEvent integrationEvent, CancellationToken cancellationToken) =>
		emailSender.SendWelcomeAsync(new SendWelcomeRequest(integrationEvent.EmailAddress, integrationEvent.FirstName), cancellationToken);
}
