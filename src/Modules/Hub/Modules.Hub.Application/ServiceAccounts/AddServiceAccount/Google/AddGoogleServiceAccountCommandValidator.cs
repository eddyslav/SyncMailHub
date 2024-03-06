namespace Modules.Hub.Application.ServiceAccounts.AddServiceAccount.Google;

internal sealed class AddGoogleServiceAccountCommandValidator : AbstractValidator<AddGoogleServiceAccountCommand>
{
	public AddGoogleServiceAccountCommandValidator()
	{
		RuleFor(x => x.Code).NotEmpty();
	}
}
