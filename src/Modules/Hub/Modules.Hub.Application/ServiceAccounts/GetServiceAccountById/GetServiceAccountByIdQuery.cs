namespace Modules.Hub.Application.ServiceAccounts.GetServiceAccountById;

public sealed record GetServiceAccountByIdQuery(ServiceAccountId AccountId) : IQuery<ServiceAccountResponse>;
