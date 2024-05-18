namespace Modules.Hub.Application.Emails.GetEmails;

internal sealed class GetEmailsQueryValidator : AbstractValidator<GetEmailsQuery>
{
	public GetEmailsQueryValidator()
	{
		RuleFor(x => x.AccountId).NotNull();

		RuleFor(x => x.ConversationId).NotEmpty();
	}
}
