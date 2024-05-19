using Microsoft.EntityFrameworkCore.ChangeTracking;

using Application.DateTimeProvider;

namespace Persistence.Interceptors;

public sealed class TrackEntitiesInterceptor(IDateTimeProvider dateTimeProvider) : SaveChangesInterceptor
{
	private static void SetCurrentPropertyValue(EntityEntry entry
		, string propertyName
		, DateTimeOffset now) =>
		entry.Property(propertyName).CurrentValue = now;

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData
		, InterceptionResult<int> result
		, CancellationToken cancellationToken = default)
	{
		if (eventData.Context is not DbContext dbContext)
		{
			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		var now = dateTimeProvider.UtcNow;

		var entitiesEntries = dbContext
			.ChangeTracker
			.Entries()
			.Where(entityEntry => entityEntry is { Entity: ICreatableEntity or IUpdatableEntity, State: EntityState.Added or EntityState.Modified });

		foreach (var entityEntry in entitiesEntries)
		{
			var entity = entityEntry.Entity;

			if (entity is ICreatableEntity && entityEntry.State is EntityState.Added)
			{
				SetCurrentPropertyValue(entityEntry, nameof(ICreatableEntity.CreatedAt), now);
			}

			if (entity is IUpdatableEntity && entityEntry.State is EntityState.Modified)
			{
				SetCurrentPropertyValue(entityEntry, nameof(IUpdatableEntity.UpdatedAt), now);
			}
		}

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}
