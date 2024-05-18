namespace Domain.Common;

public interface IUpdatableEntity
{
	DateTimeOffset? UpdatedAt { get; }
}
