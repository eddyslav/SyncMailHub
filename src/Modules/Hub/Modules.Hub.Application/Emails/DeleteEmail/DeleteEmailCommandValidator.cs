namespace Modules.Hub.Application.Emails.DeleteEmail;

internal sealed class DeleteEmailCommandValidator : AbstractValidator<DeleteEmailCommand>
{
	public DeleteEmailCommandValidator()
	{
		RuleFor(x => x.AccountId).NotNull();
		RuleFor(x => x.EmailId).NotEmpty();
	}
}
