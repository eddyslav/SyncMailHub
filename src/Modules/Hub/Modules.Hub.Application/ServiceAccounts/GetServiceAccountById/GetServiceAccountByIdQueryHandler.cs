namespace Modules.Hub.Application.ServiceAccounts.GetServiceAccountById;

internal sealed class GetServiceAccountByIdQueryHandler(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor)
	: IQueryHandler<GetServiceAccountByIdQuery, ServiceAccountResponse>
{
	private async Task<Result<ServiceAccount>> GetServiceAccountAsync(GetServiceAccountByIdQuery query, CancellationToken cancellationToken) =>
		Result.Create(await accountRepository.GetByIdAndUserIdAsync(query.AccountId, userContextAccessor.UserId, cancellationToken))
			.MapFailure(ServiceAccountErrors.NotFound);

	public Task<Result<ServiceAccountResponse>> Handle(GetServiceAccountByIdQuery query, CancellationToken cancellationToken) =>
		Result.Create(query)
			.Bind(query => GetServiceAccountAsync(query, cancellationToken))
			.Map(ServiceAccountResponse.FromEntity);
}
