namespace Modules.Sync.Infrastructure.BackgroundJobs.ProcessMailboxEmails;

internal sealed class ProcessMailboxEmailsJob(IServiceAccountRepository accountRepository
	, IServiceAccountContextAccessor accountContextAccessor
	, IContextServiceAccountSyncSessionFactory syncSessionFactory
	, IContextServiceAccountPersistenceClient mailboxPersistenceClient
	, ILogger logger)
	: ServiceAccountRelatedJob(accountRepository
		, accountContextAccessor
		, logger.ForContext<ProcessMailboxEmailsJob>())
{
	protected override async Task ExecuteAsync(IJobExecutionContext context)
	{
		var cancellationToken = context.CancellationToken;

		var syncSessionResult = await syncSessionFactory.GetSyncSessionAsync(cancellationToken);
		if (!syncSessionResult.IsSuccess)
		{
			Logger.Error("Mailbox sync session could not be created for the target account: {errorMessage}", syncSessionResult.Error);
			return;
		}

		using var syncSession = syncSessionResult.Value;

		var enumerator = syncSession.ProcessStateChangesAsync(cancellationToken)
			.WithCancellation(cancellationToken);

		await foreach (var stateChangeResult in enumerator)
		{
			if (!stateChangeResult.IsSuccess)
			{
				Logger.Error("Failed to retrieve state changes. Further execution is not possible");
				break;
			}

			await mailboxPersistenceClient.SendChangesAsync(stateChangeResult.Value, cancellationToken);
		}
	}
}
