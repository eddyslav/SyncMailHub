namespace Modules.Hub.Application.Emails;

public sealed record Email(string Id
	, string? HtmlBody
	, IReadOnlyList<Email.EmailRecipient> Recipients)
{
	public sealed record EmailRecipient(string? Name
		, string EmailAddress
		, EmailRecipient.EmailRecipientType Type)
	{
		public enum EmailRecipientType
		{
			From = 0,
			To,
			Cc,
			Bcc,
		}
	}
}
