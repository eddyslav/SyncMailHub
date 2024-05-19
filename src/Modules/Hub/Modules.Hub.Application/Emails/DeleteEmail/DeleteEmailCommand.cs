namespace Modules.Hub.Application.Emails.DeleteEmail;

public sealed record DeleteEmailCommand(ServiceAccountId AccountId, string EmailId, bool Force) : IAccountCommand;
