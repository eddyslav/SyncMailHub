using System.Diagnostics;

namespace Modules.Hub.Application.ServiceAccounts;

public sealed record ServiceAccountResponse(ServiceAccountId Id
	, string EmailAddress
	, DateTimeOffset CreatedAt
	, ServiceAccountResponse.AccountType Type)
{
	public enum AccountType
	{
		Google = 0,
	}

	public static ServiceAccountResponse FromEntity(ServiceAccount account) =>
		new(account.Id
			, account.EmailAddress
			, account.CreatedAt
			, account switch
			{
				GoogleServiceAccount => AccountType.Google,
				_ => throw new UnreachableException(),
			});
}
