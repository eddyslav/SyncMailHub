using Modules.Hub.Infrastucture.Google;

namespace Modules.Hub.Infrastucture.ServiceAccountCredentialsProvider;

internal sealed class ServiceAccountCredentialsProvider(IServiceAccountRepository accountRepository
	, IServiceProvider serviceProvider
	, IEncryptionService encryptionService)
	: IServiceAccountCredentialsProvider
{
	private async Task<Result<ServiceAccount>> GetServiceAccountAsync(ServiceAccountId accountId, CancellationToken cancellationToken) =>
		Result.Create(await accountRepository.GetByIdAsync(accountId, cancellationToken))
			.MapFailure(ServiceAccountErrors.NotFound);

	private IServiceAccountCredentials GetCredentials(ServiceAccount account)
	{
		GoogleServiceAccountCredentials GetCredentials(GoogleServiceAccount account)
		{
			var configuration = serviceProvider.GetRequiredService<IOptions<GoogleOAuthConfiguration>>()
				.GetConfiguration();

			return new GoogleServiceAccountCredentials(configuration.ClientId
				, configuration.ClientSecret
				, encryptionService.Decrypt(account.RefreshToken));
		}

		return account switch
		{
			GoogleServiceAccount googleAccount => GetCredentials(googleAccount),
			_ => throw new UnreachableException(),
		};
	}

	public Task<Result<IServiceAccountCredentials>> GetByAccountIdAsync(ServiceAccountId accountId, CancellationToken cancellationToken = default) =>
		Result.Create(accountId)
			.Bind(accountId => GetServiceAccountAsync(accountId, cancellationToken))
			.Map(GetCredentials);
}
