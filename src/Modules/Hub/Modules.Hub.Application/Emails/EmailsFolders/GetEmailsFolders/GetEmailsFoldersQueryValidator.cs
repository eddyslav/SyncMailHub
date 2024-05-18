namespace Modules.Hub.Application.Emails.EmailsFolders.GetEmailsFolders;

internal sealed class GetEmailsFoldersValidator : AbstractValidator<GetEmailsFoldersQuery>
{
	public GetEmailsFoldersValidator()
	{
		RuleFor(x => x.AccountId).NotNull();
	}
}
