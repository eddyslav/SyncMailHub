namespace Modules.Hub.Application.Emails;

internal abstract class AccountCommandHandlerBase<TCommand, TResponse>(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: ICommandHandler<TCommand, TResponse>
	where TCommand : IAccountCommand<TResponse>
	where TResponse : notnull
{
	private async Task<Result<TCommand>> CheckIfAccountExistsAsync(TCommand command, CancellationToken cancellationToken) =>
		Result.Create(await accountRepository.CheckIfExistsByIdAndUserIdAsync(command.AccountId, userContextAccessor.UserId, cancellationToken))
			.Map(() => command)
			.MapFailure(ServiceAccountErrors.NotFound);

	protected abstract Task<Result<TResponse>> HandleAsync(TCommand command
		, IMailService mailService
		, CancellationToken cancellationToken);

	public Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken) =>
		CheckIfAccountExistsAsync(command, cancellationToken)
			.Bind(command => mailServiceFactory.CreateForAccountAsync(command.AccountId, cancellationToken))
			.Bind(mailService => HandleAsync(command, mailService, cancellationToken));
}

internal abstract class AccountCommandHandlerBase<TCommand>(IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IMailServiceFactory mailServiceFactory)
	: ICommandHandler<TCommand>
	where TCommand : IAccountCommand
{
	private async Task<Result<TCommand>> CheckIfAccountExistsAsync(TCommand command, CancellationToken cancellationToken) =>
		Result.Create(await accountRepository.CheckIfExistsByIdAndUserIdAsync(command.AccountId, userContextAccessor.UserId, cancellationToken))
			.Map(() => command)
			.MapFailure(ServiceAccountErrors.NotFound);

	protected abstract Task<Result> HandleAsync(TCommand command
		, IMailService mailService
		, CancellationToken cancellationToken);

	public Task<Result> Handle(TCommand command, CancellationToken cancellationToken) =>
		CheckIfAccountExistsAsync(command, cancellationToken)
			.Bind(command => mailServiceFactory.CreateForAccountAsync(command.AccountId, cancellationToken))
			.Bind(mailService => HandleAsync(command, mailService, cancellationToken));
}
