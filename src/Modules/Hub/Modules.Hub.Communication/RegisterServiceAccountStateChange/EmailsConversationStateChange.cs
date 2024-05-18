namespace Modules.Hub.Communication.RegisterServiceAccountStateChange;

public sealed record EmailsConversationStateChange(string Id
	, IReadOnlyList<string> FolderIds)
	: IServiceAccountStateChange;
