namespace Modules.Hub.Application.Users;

public sealed record AuthenticationResponse(UserId UserId
	, string EmailAddress
	, string FirstName
	, string LastName
	, DateTimeOffset RegisteredAt
	, string Token
	, DateTimeOffset ExpiresAt);
