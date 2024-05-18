using MimeKit;
using MimeKit.Text;

using MailKit.Net.Smtp;

namespace Modules.Notifications.Infrastructure.EmailSenderClient;

internal sealed class SmtpMailSenderClient(IOptions<SmtpMailSenderClientConfiguration> options) : IMailSenderClient
{
	private readonly SmtpMailSenderClientConfiguration configuration = options.GetConfiguration();

	private static void PropagateEmailAddresses(IEnumerable<string>? addresses, InternetAddressList target) =>
		addresses?.ForEach(address => target.Add(new MailboxAddress(null, address)));

	private MimeMessage CreateMessage(SendEmailRequest request)
	{
		var message = new MimeMessage();

		message.From.Add(new MailboxAddress(configuration.FromName, configuration.FromEmailAddress));
		PropagateEmailAddresses(request.To, message.To);
		PropagateEmailAddresses(request.Cc, message.Cc);

		message.Subject = request.Subject;
		message.Body = new TextPart(TextFormat.Html)
		{
			Text = request.Body,
		};

		return message;
	}

	public async Task SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken)
	{
		var message = CreateMessage(request);

		using var smtpClient = new SmtpClient();

		await smtpClient.ConnectAsync(configuration.Host, configuration.Port, false, cancellationToken);
		await smtpClient.AuthenticateAsync(configuration.UserName, configuration.Password, cancellationToken);

		await smtpClient.SendAsync(message, cancellationToken);

		await smtpClient.DisconnectAsync(true, cancellationToken);
	}
}
