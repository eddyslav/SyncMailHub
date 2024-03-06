namespace Modules.Notifications.Infrastructure.MailSender;

internal sealed class MailSenderConfiguration
{
	public sealed record EmailTemplate(string Subject, string Template);

	public required EmailTemplate WelcomeTemplate { get; init; }

	public required EmailTemplate ServiceAccountAddedTemplate { get; init; }
}
