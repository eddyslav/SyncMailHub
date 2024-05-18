namespace Modules.Hub.Domain.Users;

public static class UserErrors
{
	public static Error EmailAddressIsRequired { get; } = new("User.EmailAddressIsRequired", "The user's email address is required");
	public static Error EmailAddressIsInvalid { get; } = new("User.EmailAddressIsInvalid", "The user's email address is invalid");
	public static Error EmailAddressMaxLengthExceeded { get; } = new("User.EmailAddressMaxLengthExceeded", "The user's email address max length is exceeded");
	public static Error FirstNameIsRequired { get; } = new("User.FirstNameIsRequired", "The user's first name is required");
	public static Error FirstNameMaxLengthExceeded { get; } = new("User.FirstNameMaxLengthExceeded", "The user's first name max length is exceeded");
	public static Error LastNameIsRequired { get; } = new("User.LastNameIsRequired", "The user's last name is required");
	public static Error LastNameMaxLengthExceeded { get; } = new("User.LastNameMaxLengthExceeded", "The user's last name max length is exceeded");
	public static Error PasswordIsRequired { get; } = new("User.PasswordIsRequired", "The user's password is required");
	public static Error InvalidCredentials { get; } = new UnauthorizedError("User.InvalidCredentials", "The user with the specified user name does not exist or invalid password provided");
	public static Error EmailAddressAlreadyTaken { get; } = new ConflictError("User.EmailAddressAlreadyTaken", "The user with the specified email address already exists");
	public static Error CurrentUserDoesNotExist { get; } = new UnauthorizedError("User.CurrentUserDoesNotExist", "The user with which credentials you are trying to execute requests does not exist");
}
