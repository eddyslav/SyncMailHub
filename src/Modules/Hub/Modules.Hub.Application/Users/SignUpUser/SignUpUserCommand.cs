namespace Modules.Hub.Application.Users.SignUpUser;

public sealed record SignUpUserCommand(string EmailAddress
	, string FirstName
	, string LastName
	, string Password) : ICommand<AuthenticationResponse>;
