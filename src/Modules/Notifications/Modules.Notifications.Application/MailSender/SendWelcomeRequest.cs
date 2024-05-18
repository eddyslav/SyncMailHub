namespace Modules.Notifications.Application.MailSender;

public sealed record SendWelcomeRequest(string EmailAddress, string FirstName);
