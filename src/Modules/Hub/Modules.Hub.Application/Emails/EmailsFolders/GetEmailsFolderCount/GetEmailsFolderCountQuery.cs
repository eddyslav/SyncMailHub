namespace Modules.Hub.Application.Emails.EmailsFolders.GetEmailsFolderCount;

public sealed record GetEmailsFolderCountQuery(ServiceAccountId AccountId, string FolderId)
	: IAccountQuery<EmailsFolderCount>;
