namespace Modules.Hub.Application.Users.SignInUser;

internal sealed class SignInUserCommandValidator : AbstractValidator<SignInUserCommand>
{
	public SignInUserCommandValidator()
	{
		RuleFor(x => x.EmailAddress)
			.NotEmpty()
			.WithError(UserErrors.EmailAddressIsRequired)
			.EmailAddress()
			.WithError(UserErrors.EmailAddressIsInvalid)
			.MaximumLength(UserConstants.EmailAddressMaxLength)
			.WithError(UserErrors.EmailAddressMaxLengthExceeded);

		RuleFor(x => x.Password)
			.NotEmpty()
			.WithError(UserErrors.PasswordIsRequired);
	}
}
