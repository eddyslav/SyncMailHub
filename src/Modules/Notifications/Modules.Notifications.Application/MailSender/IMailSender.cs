namespace Modules.Notifications.Application.MailSender;

public interface IMailSender
{
	Task SendWelcomeAsync(SendWelcomeRequest request, CancellationToken cancellationToken);

	Task SendAccountConnectedAsync(SendAccountConnectedRequest request, CancellationToken cancellationToken);
}
