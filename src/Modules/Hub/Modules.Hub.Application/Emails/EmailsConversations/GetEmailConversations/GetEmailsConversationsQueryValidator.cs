namespace Modules.Hub.Application.Emails.EmailsConversations.GetEmailConversations;

internal sealed class GetEmailsConversationsQueryValidator : AbstractValidator<GetEmailsConversationsQuery>
{
	public GetEmailsConversationsQueryValidator()
	{
		RuleFor(x => x.AccountId).NotNull();

		RuleFor(x => x.FolderId).NotEmpty();

		RuleFor(x => x.PageToken)
			.NotEmpty()
			.When(x => x.PageToken is not null);
	}
}
