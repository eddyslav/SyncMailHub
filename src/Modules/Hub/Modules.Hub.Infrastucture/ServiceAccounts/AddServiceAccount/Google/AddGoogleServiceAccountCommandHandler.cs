using Modules.Hub.Application.ServiceAccounts.AddServiceAccount;

using Modules.Hub.Application.ServiceAccounts.AddServiceAccount.Google;

namespace Modules.Hub.Infrastucture.ServiceAccounts.AddServiceAccount.Google;

internal sealed class AddGoogleServiceAccountCommandHandler(GoogleServiceAccountCredentialsVerifier credentialsVerifier
	, IServiceAccountRepository accountRepository
	, IUserContextAccessor userContextAccessor
	, IUserRepository userRepository
	, IEncryptionService encryptionService
	, IUnitOfWork unitOfWork)
	: ICommandHandler<AddGoogleServiceAccountCommand, AddServiceAccountResponse>
{
	private async Task<Result<GoogleMailboxCredentialsVerifyResult>> CheckIfMailboxExistsAsync(GoogleMailboxCredentialsVerifyResult verifyResult
		, CancellationToken cancellationToken) =>
		Result.Create(!await accountRepository.CheckIfExistsByExternalIdAsync(verifyResult.ExternalId, cancellationToken))
			.Map(() => verifyResult)
			.MapFailure(ServiceAccountErrors.AlreadyExists);

	private async Task<Result<User>> GetUserByIdAsync(UserId userId, CancellationToken cancellationToken) =>
		Result.Create(await userRepository.GetByIdAsync(userId, cancellationToken))
			.MapFailure(ServiceAccountErrors.UserNotFound);

	private Task<Result<GoogleServiceAccount>> CreateServiceAccountEntityAsync(GoogleMailboxCredentialsVerifyResult verifyResult
		, CancellationToken cancellationToken) =>
		Result.Create(userContextAccessor.UserId)
			.Bind(userId => GetUserByIdAsync(userId, cancellationToken))
			.Map(user => GoogleServiceAccount.Create(user
				, verifyResult.EmailAddress
				, verifyResult.ExternalId
				, encryptionService.Encrypt(verifyResult.RefreshToken)));

	public async Task<Result<AddServiceAccountResponse>> Handle(AddGoogleServiceAccountCommand command, CancellationToken cancellationToken) =>
		await Result.Create(command)
			.Bind(command => credentialsVerifier.VerifyCredentialsAsync(command.Code, cancellationToken))
			.Bind(verifyResult => CheckIfMailboxExistsAsync(verifyResult, cancellationToken))
			.Bind(verifyResult => CreateServiceAccountEntityAsync(verifyResult, cancellationToken))
			.Tap(accountRepository.Add)
			.Tap(() => unitOfWork.SaveChangesAsync(cancellationToken))
			.Map(serviceAccount => new AddServiceAccountResponse(serviceAccount.Id, serviceAccount.EmailAddress));
}
