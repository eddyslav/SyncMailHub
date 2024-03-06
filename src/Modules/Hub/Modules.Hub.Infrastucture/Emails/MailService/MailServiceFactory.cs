namespace Modules.Hub.Infrastucture.Emails.MailService;

internal sealed class MailServiceFactory(IServiceAccountCredentialsProvider credentialsProvider
	, IServiceProvider serviceProvider)
	: IMailServiceFactory
{
	private IMailService CreateGoogleService(GoogleServiceAccountCredentials credentials)
	{
		var logger = serviceProvider.GetRequiredService<ILogger>();
		var options = serviceProvider.GetRequiredService<IOptions<Google.MailServiceConfiguration>>();

		return new Google.MailService(credentials, logger, options);
	}

	private IMailService CreateMailService(ServiceAccountId accountId, IServiceAccountCredentials accountCredentials)
	{
		var mailServiceBase = accountCredentials switch
		{
			GoogleServiceAccountCredentials googleCredentials => CreateGoogleService(googleCredentials),
			_ => throw new UnreachableException(),
		};

		var serviceAccountContextAccessor = new ServiceAccountContextAccessor
		{
			AccountId = accountId,
		};

		var cachingService = serviceProvider.GetRequiredService<ICachingService>();
		var options = serviceProvider.GetRequiredService<IOptions<Aggregated.MailServiceConfiguration>>();

		return new Aggregated.MailService(mailServiceBase
			, cachingService
			, serviceAccountContextAccessor
			, options);
	}

	public async Task<Result<IMailService>> CreateForAccountAsync(ServiceAccountId serviceAccountId, CancellationToken cancellationToken) =>
		await credentialsProvider.GetByAccountIdAsync(serviceAccountId, cancellationToken)
			.Map(credentials => CreateMailService(serviceAccountId, credentials));
}
