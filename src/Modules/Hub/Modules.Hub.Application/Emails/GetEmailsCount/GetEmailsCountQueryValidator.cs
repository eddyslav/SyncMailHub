namespace Modules.Hub.Application.Emails.GetEmailsCount;

internal sealed class GetEmailsCountQueryValidator : AbstractValidator<GetEmailsCountQuery>
{
	public GetEmailsCountQueryValidator()
	{
		RuleFor(x => x.AccountId).NotNull();
	}
}
