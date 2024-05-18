using Modules.Hub.Domain;

namespace Modules.Hub.Persistence;

public sealed class UnitOfWork(HubDbContext dbContext) : IUnitOfWork
{
	public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
		await dbContext.SaveChangesAsync(cancellationToken);
}
