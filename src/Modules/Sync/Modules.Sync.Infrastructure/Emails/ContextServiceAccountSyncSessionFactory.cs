namespace Modules.Sync.Infrastructure.Emails;

internal sealed class ContextServiceAccountSyncSessionFactory(IContextServiceAccountCredentialsProvider credentialsProvider
	, IServiceProvider serviceProvider)
	: IContextServiceAccountSyncSessionFactory
{
	private Google.ServiceAccountSyncSession CreateGoogleSession(GoogleServiceAccountCredentials credentials)
	{
		var accountContextAccessor = serviceProvider.GetRequiredService<IServiceAccountContextAccessor>();
		var syncStateRepository = serviceProvider.GetRequiredService<IServiceAccountSyncStateRepository>();
		var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
		var logger = serviceProvider.GetRequiredService<ILogger>();
		var options = serviceProvider.GetRequiredService<IOptions<Google.ServiceAccountSyncSessionConfiguration>>();

		return new Google.ServiceAccountSyncSession(credentials
			, accountContextAccessor
			, syncStateRepository
			, unitOfWork
			, logger
			, options);
	}

	private IServiceAccountSyncSession CreateSession(IServiceAccountCredentials accountCredentials) =>
		accountCredentials switch
		{
			GoogleServiceAccountCredentials googleCredentials => CreateGoogleSession(googleCredentials),
			_ => throw new UnreachableException(),
		};

	public Task<Result<IServiceAccountSyncSession>> GetSyncSessionAsync(CancellationToken cancellationToken = default) =>
		credentialsProvider.GetCredentialsAsync(cancellationToken)
			.Map(CreateSession);
}
