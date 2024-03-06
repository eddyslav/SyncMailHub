namespace Modules.Hub.Domain;

public interface IUnitOfWork
{
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
