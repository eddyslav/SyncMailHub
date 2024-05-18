namespace Modules.Sync.Application.Emails;

public sealed record EmailsConversationArtifactStateChange(string Id, IReadOnlyList<string> FolderIds) : IServiceAccountStateChange;
