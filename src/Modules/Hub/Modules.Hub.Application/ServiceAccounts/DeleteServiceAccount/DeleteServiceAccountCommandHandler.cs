namespace Modules.Hub.Application.ServiceAccounts.DeleteServiceAccount;

internal sealed class DeleteServiceAccountCommandHandler(IServiceAccountRepository accountRepository
	, IUnitOfWork unitOfWork)
	: ICommandHandler<DeleteServiceAccountCommand>
{
	private async Task<Result<ServiceAccount>> GetAccountByIdAsync(DeleteServiceAccountCommand command, CancellationToken cancellationToken) =>
		Result.Create(await accountRepository.GetByIdAsync(command.Id, cancellationToken))
			.MapFailure(ServiceAccountErrors.NotFound);

	public async Task<Result> Handle(DeleteServiceAccountCommand command, CancellationToken cancellationToken) =>
		await Result.Create(command)
			.Bind(command => GetAccountByIdAsync(command, cancellationToken))
			.Tap(account => account.DeleteAccount())
			.Tap(accountRepository.Remove)
			.Tap(() => unitOfWork.SaveChangesAsync(cancellationToken));
}
