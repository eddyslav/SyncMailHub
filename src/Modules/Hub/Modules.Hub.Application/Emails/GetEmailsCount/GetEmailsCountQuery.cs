namespace Modules.Hub.Application.Emails.GetEmailsCount;

public sealed record GetEmailsCountQuery(ServiceAccountId AccountId) : IAccountQuery<EmailsCounter>;
