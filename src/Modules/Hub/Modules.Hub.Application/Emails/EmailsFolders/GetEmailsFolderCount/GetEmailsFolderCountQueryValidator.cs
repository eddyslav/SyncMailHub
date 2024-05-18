namespace Modules.Hub.Application.Emails.EmailsFolders.GetEmailsFolderCount;

internal sealed class GetEmailsFolderCountQueryValidator : AbstractValidator<GetEmailsFolderCountQuery>
{
	public GetEmailsFolderCountQueryValidator()
	{
		RuleFor(x => x.AccountId).NotNull();

		RuleFor(x => x.FolderId).NotEmpty();
	}
}
