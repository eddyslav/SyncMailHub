namespace Modules.Notifications.Application.MailSender;

public sealed record SendAccountConnectedRequest(string EmailAddress, string FirstName);
