using Modules.Hub.Communication.GetAccountCredentials;

namespace Modules.Hub.Infrastucture.Communication;

internal sealed class GetAccountCredentialsConsumer(IServiceAccountCredentialsProvider credentialsProvider) : IConsumer<GetAccountCredentialsRequest>
{
	private static GetAccountCredentialsResponseBase MapToModel(IServiceAccountCredentials accountCredentials) =>
		accountCredentials switch
		{
			GoogleServiceAccountCredentials googleCredentials =>
				new GoogleGetAccountCredentialsResponse(googleCredentials.ClientId
					, googleCredentials.ClientSecret
					, googleCredentials.RefreshToken),
			_ => throw new UnreachableException(),
		};

	public async Task Consume(ConsumeContext<GetAccountCredentialsRequest> context) =>
		await Result.Create(context)
			.Bind(context => credentialsProvider.GetByAccountIdAsync(new ServiceAccountId(context.Message.AccountId)))
			.Map(MapToModel)
			.Tap(response => context.RespondAsync(response))
			.OnFailure(error => context.RespondAsync(error));
}
