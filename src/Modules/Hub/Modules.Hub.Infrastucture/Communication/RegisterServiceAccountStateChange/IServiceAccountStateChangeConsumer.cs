namespace Modules.Hub.Infrastucture.Communication.RegisterServiceAccountStateChange;

internal interface IServiceAccountStateChangeConsumer<TStateChange> : IServiceAccountStateChangeConsumer
	where TStateChange : IServiceAccountStateChange
{
	Task HandleAsync(ServiceAccountId accountId
		, TStateChange stateChange
		, CancellationToken cancellationToken);
}

internal interface IServiceAccountStateChangeConsumer
{
	Task HandleAsync(ServiceAccountId accountId
		, IServiceAccountStateChange stateChange
		, CancellationToken cancellationToken);
}
