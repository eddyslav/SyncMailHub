namespace Modules.Hub.Application.Users.SignInUser;

public sealed record SignInUserCommand(string EmailAddress, string Password) : ICommand<AuthenticationResponse>;
