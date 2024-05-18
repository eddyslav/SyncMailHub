namespace Modules.Hub.Application.Emails;

public sealed record EmailsFolder(string Id
	, string Name
	, string? ParentFolderId);
