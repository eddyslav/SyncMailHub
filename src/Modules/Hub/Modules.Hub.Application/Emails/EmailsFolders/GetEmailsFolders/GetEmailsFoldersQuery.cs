namespace Modules.Hub.Application.Emails.EmailsFolders.GetEmailsFolders;

public sealed record GetEmailsFoldersQuery(ServiceAccountId AccountId) : IAccountQuery<IReadOnlyList<EmailsFolder>>;
