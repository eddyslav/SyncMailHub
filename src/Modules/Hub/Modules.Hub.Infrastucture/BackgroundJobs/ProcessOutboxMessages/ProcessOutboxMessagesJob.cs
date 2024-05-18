using MediatR;

using Newtonsoft.Json;

using Polly;
using Polly.Retry;

using Quartz;

namespace Modules.Hub.Infrastucture.BackgroundJobs.ProcessOutboxMessages;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob(HubDbContext dbContext
	, IPublisher publisher
	, IDateTimeProvider dateTimeProvider
	, ILogger logger
	, IOptions<ProcessOutboxMessagesConfiguration> options) : IJob
{
	private static readonly JsonSerializerSettings jsonSerializerSettings = new()
	{
		TypeNameHandling = TypeNameHandling.All
	};

	private readonly ILogger logger = logger.ForContext<ProcessOutboxMessagesJob>();
	private readonly ProcessOutboxMessagesConfiguration configuration = options.GetConfiguration();

	private async Task<IReadOnlyCollection<OutboxMessage>> GetOutboxMessagesAsync(CancellationToken cancellationToken) =>
		await dbContext.Set<OutboxMessage>()
			.Where(x => !x.ProcessedAt.HasValue)
			.OrderBy(x => x.OccuredAt)
			.Take(configuration.BatchSize)
			.ToListAsync(cancellationToken);

	private ResiliencePipeline BuildResiliencePipeline() =>
		new ResiliencePipelineBuilder()
			.AddRetry(new RetryStrategyOptions
			{
				MaxRetryAttempts = configuration.MessageHandlersRetriesCount,
			})
			.Build();

	private async Task ExecuteWithPipelineAsync(ResiliencePipeline pipeline
		, IDomainEvent domainEvent
		, CancellationToken cancellationToken)
	{
		var state = (Publisher: publisher, DomainEvent: domainEvent);

		await pipeline.ExecuteAsync(async (state, cancellationToken) => await state.Publisher.Publish(state.DomainEvent, cancellationToken)
			, state
			, cancellationToken);
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var cancellationToken = context.CancellationToken;

		var outboxMessages = await GetOutboxMessagesAsync(cancellationToken);

		if (!outboxMessages.Any())
		{
			logger.Debug("No new outbox messages to be processed");
			return;
		}

		logger.Debug("There are {messagesCount} outbox message(-s) to be processed", outboxMessages.Count);

		var resiliencePipeline = BuildResiliencePipeline();

		foreach (var outboxMessage in outboxMessages)
		{
			outboxMessage.ProcessedAt = dateTimeProvider.UtcNow;

			if (JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, jsonSerializerSettings) is not { } domainEvent)
			{
				logger.Warning("{messageId} outbox message cannot be deserialized as a domain event object", outboxMessage.Id);

				outboxMessage.ErrorMessage = "Outbox message cannot be deserialized as a domain event object";
				await dbContext.SaveChangesAsync(cancellationToken);

				continue;
			}

			try
			{
				await ExecuteWithPipelineAsync(resiliencePipeline, domainEvent, cancellationToken);

				logger.Debug("{messageId} outbox message was successfully processed", outboxMessage.Id);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Failed to process {messageId} outbox message", outboxMessage.Id);

				outboxMessage.ErrorMessage = ex.Message;
			}

			await dbContext.SaveChangesAsync(cancellationToken);
		}

		logger.Debug("{messagesCount} outbox message(-s) were processed", outboxMessages.Count);
	}
}
