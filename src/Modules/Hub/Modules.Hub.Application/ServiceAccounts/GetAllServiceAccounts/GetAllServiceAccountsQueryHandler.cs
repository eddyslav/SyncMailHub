namespace Modules.Hub.Application.ServiceAccounts.GetAllServiceAccounts;

internal sealed class GetAllServiceAccountsQueryHandler(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor)
	: IQueryHandler<GetAllServiceAccountsQuery, IReadOnlyList<ServiceAccountResponse>>
{
	public Task<Result<IReadOnlyList<ServiceAccountResponse>>> Handle(GetAllServiceAccountsQuery request, CancellationToken cancellationToken) =>
		Result.Success()
			.Map(() => accountRepository.GetAllAccountsPerUserAsync(userContextAccessor.UserId, cancellationToken))
			.Map(accounts =>
				accounts.Select(account =>
					new ServiceAccountResponse(account.Id, account.EmailAddress, account.CreatedAt))
				.ToList() as IReadOnlyList<ServiceAccountResponse>);
}
