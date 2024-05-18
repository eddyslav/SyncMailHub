using System.Collections.Concurrent;

using Newtonsoft.Json;

using Polly;
using Polly.Retry;

using Quartz;

using Serilog;

namespace Modules.Notifications.Infrastructure.BackgroundJobs.ProcessInboxMessages;

[DisallowConcurrentExecution]
internal sealed class ProcessInboxMessagesJob(NotificationsDbContext dbContext
	, IDateTimeProvider dateTimeProvider
	, IServiceProvider serviceProvider
	, ILogger logger
	, IOptions<ProcessInboxMessagesConfiguration> options) : IJob
{
	private static class IntegrationEventHandlerFactory
	{
		private static readonly ConcurrentDictionary<Type, List<Type>> EventHandlersDictionary = [];

		private static void AddHandlersToDictionary(Type type) =>
			Application.AssemblyMarker.Assembly
				.GetTypes()
				.Where(EventHandlersUtils.ImplementsIntegrationEventHandler)
				.ForEach(integrationEventHandlerType =>
				{
					var closedIntegrationEventHandler = integrationEventHandlerType
						.GetInterfaces()
						.First(EventHandlersUtils.IsIntegrationEventHandler);

					var arguments = closedIntegrationEventHandler.GetGenericArguments();

					if (arguments[0] != type)
					{
						return;
					}

					EventHandlersDictionary.AddOrUpdate(
						type,
						_ => [integrationEventHandlerType],
						(_, handlersList) =>
						{
							handlersList.Add(integrationEventHandlerType);

							return handlersList;
						});
				});

		public static IEnumerable<IIntegrationEventHandler> GetHandlers(Type type, IServiceProvider serviceProvider)
		{
			if (!EventHandlersDictionary.ContainsKey(type))
			{
				AddHandlersToDictionary(type);
			}

			foreach (var eventHandlerType in EventHandlersDictionary[type])
			{
				yield return (serviceProvider.GetRequiredService(eventHandlerType) as IIntegrationEventHandler)!;
			}
		}
	}

	private static readonly JsonSerializerSettings jsonSerializerSettings = new()
	{
		TypeNameHandling = TypeNameHandling.All
	};

	private static async Task ExecuteWithPipelineAsync(ResiliencePipeline pipeline
		, IEnumerable<IIntegrationEventHandler> eventHandlers
		, IIntegrationEvent integrationEvent
		, CancellationToken cancellationToken)
	{
		var state = (EventHandlers: eventHandlers, IntegrationEvent: integrationEvent);

		await pipeline.ExecuteAsync(async (state, cancellationToken) =>
		{
			foreach (var eventHandler in state.EventHandlers)
			{
				await eventHandler.Handle(state.IntegrationEvent, cancellationToken);
			}
		}
			, state
			, cancellationToken);
	}

	private readonly ILogger logger = logger.ForContext<ProcessInboxMessagesJob>();
	private readonly ProcessInboxMessagesConfiguration configuration = options.GetConfiguration();

	private async Task<IReadOnlyCollection<InboxMessage>> GetInboxMessagesAsync(CancellationToken cancellationToken) =>
		await dbContext.Set<InboxMessage>()
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

	public async Task Execute(IJobExecutionContext context)
	{
		var cancellationToken = context.CancellationToken;

		var inboxMessages = await GetInboxMessagesAsync(cancellationToken);

		if (!inboxMessages.Any())
		{
			logger.Debug("No new inbox messages to be processed");
			return;
		}

		logger.Debug("There are {messagesCount} inbox message(-s) to be processed", inboxMessages.Count);

		var resiliencePipeline = BuildResiliencePipeline();

		foreach (var inboxMessage in inboxMessages)
		{
			inboxMessage.ProcessedAt = dateTimeProvider.UtcNow;

			if (JsonConvert.DeserializeObject<IIntegrationEvent>(inboxMessage.Content, jsonSerializerSettings) is not { } integrationEvent)
			{
				logger.Warning("{messageId} inbox message cannot be deserialized as an integration event object", inboxMessage.Id);

				inboxMessage.ErrorMessage = "Message cannot be deserialized as an integration event object";
				await dbContext.SaveChangesAsync(cancellationToken);

				continue;
			}

			var integrationEventType = integrationEvent.GetType();
			var integrationEventHandlers = IntegrationEventHandlerFactory.GetHandlers(integrationEventType, serviceProvider);

			try
			{
				await ExecuteWithPipelineAsync(resiliencePipeline
					, integrationEventHandlers
					, integrationEvent
					, cancellationToken);

				logger.Debug("{messageId} inbox message was successfully processed", inboxMessage.Id);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Failed to process {messageId} inbox message", inboxMessage.Id);

				inboxMessage.ErrorMessage = ex.Message;
			}

			await dbContext.SaveChangesAsync(cancellationToken);
		}

		logger.Debug("{messagesCount} inbox message(-s) were processed", inboxMessages.Count);
	}
}
