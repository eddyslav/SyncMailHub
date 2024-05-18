namespace Modules.Hub.Application.ServiceAccounts.DeleteServiceAccount;

public sealed record DeleteServiceAccountCommand(ServiceAccountId Id) : ICommand;
