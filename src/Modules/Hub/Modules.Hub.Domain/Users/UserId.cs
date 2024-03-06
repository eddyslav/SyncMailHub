namespace Modules.Hub.Domain.Users;

public sealed record UserId(Guid Value) : IEntityId;
