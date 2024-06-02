using Modules.Sync.Infrastructure.BackgroundJobs.ProcessMailboxEmails;

namespace Modules.Sync.Infrastructure.BackgroundJobs.UpdateSyncSchedules;

internal sealed class UpdateSyncSchedulesJob(IServiceAccountRepository accountRepository
	, ILogger logger
	, IOptions<UpdateSyncSchedulesConfiguration> options) : IJob
{
	private static readonly IEnumerable<IJobDetail> syncingJobs = [
		JobBuilder.Create(typeof(ProcessMailboxEmailsJob))
			.DisallowConcurrentExecution()
			.Build(),
	];

	private readonly ILogger logger = logger.ForContext<UpdateSyncSchedulesJob>();
	private readonly UpdateSyncSchedulesConfiguration configuration = options.GetConfiguration();

	private IReadOnlyDictionary<IJobDetail, IReadOnlyCollection<ITrigger>> CreateJobs(IEnumerable<ServiceAccount> accounts)
	{
		ITrigger CreateTriggerForAccount(IJobDetail job, ServiceAccount account)
		{
			return TriggerBuilder.Create()
				.ForJob(job)
				.WithIdentity(account.Id.Value.ToString(), job.Key.ToString())
				.UsingJobData(ContextDataKeys.AccountIdKeyName, account.Id.Value)
				.UsingJobData(ContextDataKeys.AccountHubIdKeyName, account.HubId)
				.WithSimpleSchedule(scheduleBuilder =>
					scheduleBuilder.WithInterval(configuration.JobsSchedule)
						.RepeatForever())
				.Build();
		}

		return syncingJobs.ToDictionary(job => job
			, job => accounts
				.Select(account => CreateTriggerForAccount(job, account))
				.ToList() as IReadOnlyCollection<ITrigger>);
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var previousRunDateTime = context.PreviousFireTimeUtc;
		var cancellationToken = context.CancellationToken;

		if (previousRunDateTime.HasValue)
		{
			logger.Debug("Job was run at {previousRunDateTime}. Only new accounts are to be scheduled", previousRunDateTime);
		}
		else
		{
			logger.Debug("Job wasn't run before, all accounts are to be scheduled");
		}

		var totalScheduled = 0;

		var maxAccountsPerTransaction = configuration.MaxAccountsPerTransaction;

		var serviceAccountsEnumerable = previousRunDateTime.HasValue
			? accountRepository.GetServiceAccountsCreatedAfterAsync(maxAccountsPerTransaction, previousRunDateTime.Value, cancellationToken)
			: accountRepository.GetAllServiceAccountsAsync(maxAccountsPerTransaction, cancellationToken);

		var scheduler = context.Scheduler;

		await foreach (var serviceAccounts in serviceAccountsEnumerable)
		{
			var jobsToSchedule = CreateJobs(serviceAccounts);

			await scheduler.ScheduleJobs(jobsToSchedule
				, false
				, cancellationToken);

			totalScheduled += serviceAccounts.Count;
		}

		logger.Debug("{accountsCount} account(-s) were scheduled for syncing", totalScheduled);
	}
}
