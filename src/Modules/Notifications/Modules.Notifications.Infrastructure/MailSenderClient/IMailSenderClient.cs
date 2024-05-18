namespace Modules.Notifications.Infrastructure.EmailSenderClient;

internal interface IMailSenderClient
{
	Task SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken);
}
