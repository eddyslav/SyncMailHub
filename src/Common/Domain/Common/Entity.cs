namespace Domain.Common;

public abstract class Entity<TKey> : IEntity
	where TKey : notnull, IEntityId
{
	protected Entity(TKey id)
	{
		Id = id;
	}

	// for EF core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	protected Entity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{

	}

	public TKey Id { get; private init; }

	Guid IEntity.Id => Id.Value;
}
