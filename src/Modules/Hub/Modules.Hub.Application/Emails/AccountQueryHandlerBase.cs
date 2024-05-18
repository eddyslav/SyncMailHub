namespace Modules.Hub.Application.Emails;

internal abstract class AccountQueryHandlerBase<TQuery, TResponse>(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: IQueryHandler<TQuery, TResponse>
	where TQuery : IAccountQuery<TResponse>
	where TResponse : notnull
{
	private async Task<Result<TQuery>> CheckIfAccountExistsAsync(TQuery query, CancellationToken cancellationToken) =>
		Result.Create(await accountRepository.CheckIfExistsByIdAndUserIdAsync(query.AccountId, userContextAccessor.UserId, cancellationToken))
			.Map(() => query)
			.MapFailure(ServiceAccountErrors.NotFound);

	protected abstract Task<Result<TResponse>> HandleAsync(TQuery query
		, IMailService mailService
		, CancellationToken cancellationToken);

	public Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken) =>
		CheckIfAccountExistsAsync(query, cancellationToken)
			.Bind(query => mailServiceFactory.CreateForAccountAsync(query.AccountId, cancellationToken))
			.Bind(mailService => HandleAsync(query, mailService, cancellationToken));
}
