namespace Modules.Hub.Communication.RegisterServiceAccountStateChange;

public sealed record RegisterMailboxStateChangeRequest(Guid AccountId, IReadOnlyList<IServiceAccountStateChange> Changes);
