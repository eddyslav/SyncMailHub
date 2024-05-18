namespace Modules.Hub.Application.Users;

public sealed record CurrentUserResponse(UserId Id
	, string EmailAddress
	, string FirstName
	, string LastName
	, DateTimeOffset RegisteredAt);
