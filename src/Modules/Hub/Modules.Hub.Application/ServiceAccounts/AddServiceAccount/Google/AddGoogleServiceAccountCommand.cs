namespace Modules.Hub.Application.ServiceAccounts.AddServiceAccount.Google;

public sealed record AddGoogleServiceAccountCommand(string Code, string State) : IAddServiceAccountCommand;
