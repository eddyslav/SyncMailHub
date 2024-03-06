namespace Modules.Notifications.Infrastructure.EmailSenderClient;

internal sealed class SmtpMailSenderClientConfiguration
{
	public required string FromName { get; init; }

	public required string FromEmailAddress { get; init; }

	public required string Host { get; init; }

	public required int Port { get; init; }

	public required string UserName { get; init; }

	public required string Password { get; init; }
}
