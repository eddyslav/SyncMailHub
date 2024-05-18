using Modules.Hub.Application.ServiceAccounts.AddServiceAccount;

using Modules.Hub.Application.ServiceAccounts.AddServiceAccount.Google;

namespace Modules.Hub.Infrastucture.ServiceAccounts.AddServiceAccount.Google;

internal sealed class AddGoogleServiceAccountCommandHandler(
	IHttpContextAccessor httpContextAccessor
	, GoogleServiceAccountCredentialsVerifier credentialsVerifier
	, IServiceAccountRepository accountRepository
	, IUserRepository userRepository
	, IEncryptionService encryptionService
	, IUnitOfWork unitOfWork)
	: ICommandHandler<AddGoogleServiceAccountCommand, AddServiceAccountResponse>
{
	private Result<UserId> GetUserIdFromSession() =>
		httpContextAccessor.GetSession()
			.GetGuidValue(SessionKeys.UserId)
			.Map(rawUserId => new UserId(rawUserId));

	private Result<AddGoogleServiceAccountCommand> VerifyState(AddGoogleServiceAccountCommand command) =>
		httpContextAccessor.GetSession()
			.GetStringValue(SessionKeys.State)
			.Bind(stateSessionValue => Result.Create(command.State == stateSessionValue))
			.Map(() => command)
			.MapFailure(ServiceAccountErrors.Google.StateMismatch);

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
		GetUserIdFromSession()
			.Bind(userId => GetUserByIdAsync(userId, cancellationToken))
			.Map(user => GoogleServiceAccount.Create(user
				, verifyResult.EmailAddress
				, verifyResult.ExternalId
				, encryptionService.Encrypt(verifyResult.RefreshToken)));

	public async Task<Result<AddServiceAccountResponse>> Handle(AddGoogleServiceAccountCommand command, CancellationToken cancellationToken) =>
		await Result.Create(command)
			.Bind(VerifyState)
			.Bind(command => credentialsVerifier.VerifyCredentialsAsync(command.Code, cancellationToken))
			.Bind(verifyResult => CheckIfMailboxExistsAsync(verifyResult, cancellationToken))
			.Bind(verifyResult => CreateServiceAccountEntityAsync(verifyResult, cancellationToken))
			.Tap(accountRepository.Add)
			.Tap(() => unitOfWork.SaveChangesAsync(cancellationToken))
			.Map(serviceAccount => new AddServiceAccountResponse(serviceAccount.Id, serviceAccount.EmailAddress));
}
