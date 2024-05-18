namespace Modules.Sync.Infrastructure.ServiceAccountCredentialsProvider;

internal sealed class HubServiceAccountCredentialsProvider(IRequestClient<GetAccountCredentialsRequest> requestClient) : IServiceAccountCredentialsProvider
{
	public async Task<Result<IServiceAccountCredentials>> GetByServiceAccountIdAsync(Guid hubId, CancellationToken cancellationToken = default)
	{
		var credentialsRequest = new GetAccountCredentialsRequest(hubId);
		var response = await requestClient.GetResponse<GetAccountCredentialsResponseBase, Error>(credentialsRequest, cancellationToken);

		if (response.Is<Error>(out var errorResponse))
		{
			return Result.Failure<IServiceAccountCredentials>(errorResponse.Message);
		}

		if (!response.Is<GetAccountCredentialsResponseBase>(out var credentialsResponse))
		{
			throw new UnreachableException();
		}

		return credentialsResponse.Message switch
		{
			GoogleGetAccountCredentialsResponse googleCredentials =>
				new GoogleServiceAccountCredentials(googleCredentials.ClientId
					, googleCredentials.ClientSecret
					, googleCredentials.RefreshToken),
			_ => throw new UnreachableException(),
		};
	}
}
