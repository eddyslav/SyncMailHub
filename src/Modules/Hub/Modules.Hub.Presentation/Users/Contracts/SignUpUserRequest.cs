namespace Modules.Hub.Presentation.Users.Contracts;

public sealed record SignUpUserRequest(string UserName
	, string EmailAddress
	, string FirstName
	, string LastName
	, string Password);
