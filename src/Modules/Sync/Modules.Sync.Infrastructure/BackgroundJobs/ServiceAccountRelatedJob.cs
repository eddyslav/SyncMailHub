namespace Modules.Sync.Infrastructure.BackgroundJobs;

internal abstract class ServiceAccountRelatedJob(IServiceAccountRepository accountRepository
	, IServiceAccountContextAccessor serviceAccountContextAccessor
	, ILogger logger) : IJob
{
	protected IServiceAccountContextAccessor ServiceAccountContextAccessor = serviceAccountContextAccessor;
	protected ILogger Logger { get; private set; } = logger;

	protected abstract Task ExecuteAsync(IJobExecutionContext context);

	public async Task Execute(IJobExecutionContext context)
	{
		var dataMap = context.MergedJobDataMap;

		if (!dataMap.TryGetGuid(ContextDataKeys.AccountIdKeyName, out var rawAccountId))
		{
			Logger.Error("Context service account id was not set or is not a valid guid. Skipping the execution");
			return;
		}

		if (!dataMap.TryGetGuid(ContextDataKeys.AccountHubIdKeyName, out var accountHubId))
		{
			Logger.Error("Context service account hub id was not set or is not a valid guid. Skipping the execution");
			return;
		}

		Logger = Logger.ForContext("jobId", context.JobDetail.Key.ToString())
			.ForContext("serviceAccountId", accountHubId);

		var cancellationToken = context.CancellationToken;

		var serviceAccount = await accountRepository.GetByIdAsync(new ServiceAccountId(rawAccountId), cancellationToken);
		if (serviceAccount is null)
		{
			Logger.Error("Context service account cannot be found in the data store. Skipping the execution and removing trigger");

			if (!await context.Scheduler.UnscheduleJob(context.Trigger.Key, cancellationToken))
			{
				Logger.Error("Failed to remove the trigger");
				return;
			}

			return;
		}

		ServiceAccountContextAccessor.Account = serviceAccount;

		await ExecuteAsync(context);
	}
}
