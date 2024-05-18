namespace Modules.Hub.Infrastucture.Communication.RegisterServiceAccountStateChange;

internal abstract class ServiceAccountStateChangeConsumerBase<TStateChange> : IServiceAccountStateChangeConsumer<TStateChange>
	where TStateChange : IServiceAccountStateChange
{
	public abstract Task HandleAsync(ServiceAccountId accountId
		, TStateChange stateChange
		, CancellationToken cancellationToken);

	public Task HandleAsync(ServiceAccountId accountId
		, IServiceAccountStateChange stateChange
		, CancellationToken cancellationToken) =>
		HandleAsync(accountId, (TStateChange)stateChange, cancellationToken);
}
