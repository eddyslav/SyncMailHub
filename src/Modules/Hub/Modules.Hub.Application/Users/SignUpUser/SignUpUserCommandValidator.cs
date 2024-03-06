namespace Modules.Hub.Application.Users.SignUpUser;

internal sealed class SignUpUserCommandValidator : AbstractValidator<SignUpUserCommand>
{
	public SignUpUserCommandValidator()
	{
		RuleFor(x => x.EmailAddress)
			.NotEmpty()
			.WithError(UserErrors.EmailAddressIsRequired)
			.EmailAddress()
			.WithError(UserErrors.EmailAddressIsInvalid)
			.MaximumLength(UserConstants.EmailAddressMaxLength)
			.WithError(UserErrors.EmailAddressMaxLengthExceeded);

		RuleFor(x => x.FirstName)
			.NotEmpty()
			.WithError(UserErrors.FirstNameIsRequired)
			.MaximumLength(UserConstants.FirstNameMaxLength)
			.WithError(UserErrors.FirstNameMaxLengthExceeded);

		RuleFor(x => x.LastName)
			.NotEmpty()
			.WithError(UserErrors.LastNameIsRequired)
			.MaximumLength(UserConstants.LastNameMaxLength)
			.WithError(UserErrors.LastNameMaxLengthExceeded);

		RuleFor(x => x.Password)
			.NotEmpty()
			.WithError(UserErrors.PasswordIsRequired);
	}
}
