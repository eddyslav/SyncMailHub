using System.Collections.Concurrent;

namespace Modules.Hub.Infrastucture.Communication.RegisterServiceAccountStateChange;

internal sealed class RegisterServiceAccountStateChangeConsumer(IServiceProvider serviceProvider
	, ILogger logger) : IConsumer<RegisterMailboxStateChangeRequest>
{
	private readonly ILogger logger = logger.ForContext<RegisterServiceAccountStateChangeConsumer>();

	private static class ServiceAccountStateChangeConsumerFactory
	{
		private static readonly Type changeConsumerGenericType = typeof(IServiceAccountStateChangeConsumer<>);
		private static readonly ConcurrentDictionary<Type, List<Type>> changeConsumersDictionary = [];

		private static bool IsServiceAccountStateChangeConsumer(Type interfaceType) =>
			interfaceType.IsGenericType
				&& interfaceType.GetGenericTypeDefinition() == changeConsumerGenericType;

		private static bool ImplementsServiceAccountStateChangeConsumer(Type type) =>
			type.GetInterfaces()
				.Any(IsServiceAccountStateChangeConsumer);

		private static void AddHandlersToDictionary(Type stateChangeType) =>
			typeof(RegisterServiceAccountStateChangeConsumer).Assembly
				.GetTypes()
				.Where(ImplementsServiceAccountStateChangeConsumer)
				.ForEach(stateChangeConsumerType =>
				{
					var closedIntegrationEventHandler = stateChangeConsumerType
						.GetInterfaces()
						.First(IsServiceAccountStateChangeConsumer);

					var arguments = closedIntegrationEventHandler.GetGenericArguments();

					if (arguments[0] != stateChangeType)
					{
						return;
					}

					changeConsumersDictionary.AddOrUpdate(
						stateChangeType,
						_ => [stateChangeConsumerType],
						(_, handlersList) =>
						{
							handlersList.Add(stateChangeConsumerType);

							return handlersList;
						});
				});

		public static IEnumerable<IServiceAccountStateChangeConsumer> GetConsumers(Type changeType, IServiceProvider serviceProvider)
		{
			if (!changeConsumersDictionary.ContainsKey(changeType))
			{
				AddHandlersToDictionary(changeType);
			}

			foreach (var eventHandlerType in changeConsumersDictionary[changeType])
			{
				yield return (serviceProvider.GetRequiredService(eventHandlerType) as IServiceAccountStateChangeConsumer)!;
			}
		}
	}

	private async Task<Result> HandleChangesByTypeAsync(ServiceAccountId accountId
		, IEnumerable<IServiceAccountStateChange> changes
		, CancellationToken cancellationToken)
	{
		foreach (var change in changes)
		{
			var changeType = change.GetType();

			logger.Debug("{stateChangeType} state change type sync action to be executed", changeType.Name);

			var consumers = ServiceAccountStateChangeConsumerFactory.GetConsumers(changeType, serviceProvider);
			foreach (var consumer in consumers)
			{
				await consumer.HandleAsync(accountId, change, cancellationToken);
			}
		}

		return Result.Success();
	}

	public async Task Consume(ConsumeContext<RegisterMailboxStateChangeRequest> context)
	{
		var request = context.Message;
		var cancellationToken = context.CancellationToken;

		var rawAccountId = request.AccountId;
		var accountId = new ServiceAccountId(rawAccountId);

		using var _ = LogContext.PushProperty("accountId", rawAccountId);

		await HandleChangesByTypeAsync(accountId, request.Changes, cancellationToken)
			.Tap(() => context.RespondAsync(RegisterMailboxStateChangeResponse.Instance))
			.OnFailure(error => context.RespondAsync(error));
	}
}
