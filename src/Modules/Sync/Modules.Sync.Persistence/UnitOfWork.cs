using Modules.Sync.Domain;

namespace Modules.Sync.Persistence;

public sealed class UnitOfWork(SyncDbContext dbContext) : IUnitOfWork
{
	public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
		await dbContext.SaveChangesAsync(cancellationToken);
}
