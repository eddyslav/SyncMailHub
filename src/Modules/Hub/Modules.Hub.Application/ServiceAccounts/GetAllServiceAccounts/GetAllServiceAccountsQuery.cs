namespace Modules.Hub.Application.ServiceAccounts.GetAllServiceAccounts;

public sealed class GetAllServiceAccountsQuery : IQuery<IReadOnlyList<ServiceAccountResponse>>
{
	private GetAllServiceAccountsQuery()
	{
	}

	public static GetAllServiceAccountsQuery Instance { get; } = new();
}
