namespace Modules.Sync.Application.Emails;

public interface IContextServiceAccountPersistenceClient
{
	Task<Result> SendChangesAsync(IReadOnlyList<IServiceAccountStateChange> changes, CancellationToken cancellationToken = default);
}
