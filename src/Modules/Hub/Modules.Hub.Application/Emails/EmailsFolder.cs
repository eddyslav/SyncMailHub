namespace Modules.Hub.Application.Emails;

public sealed record EmailsFolder(string Id
	, string Name
	, IReadOnlyList<EmailsFolder>? Children);
