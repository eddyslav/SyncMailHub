namespace Modules.Hub.Application.Users.SignInUser;

internal sealed class SignInUserCommandHandler(IUserRepository userRepository
	, IPasswordHasherService passwordHasher
	, ITokenService tokenService) : ICommandHandler<SignInUserCommand, AuthenticationResponse>
{
	private async Task<Result<User>> GetUserByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken) =>
		Result.Create(await userRepository.GetByEmailAddressAsync(emailAddress, cancellationToken))
			.MapFailure(UserErrors.InvalidCredentials);

	private Result<User> CheckIfPasswordsMatch(User user, string password) =>
		Result.Create(passwordHasher.VerifyPassword(password, user.Password))
			.Map(() => user)
			.MapFailure(UserErrors.InvalidCredentials);

	public async Task<Result<AuthenticationResponse>> Handle(SignInUserCommand command, CancellationToken cancellationToken) =>
		await Result.Create(command)
			.Bind(command => GetUserByEmailAddressAsync(command.EmailAddress, cancellationToken))
			.Bind(user => CheckIfPasswordsMatch(user, command.Password))
			.Map(user => (User: user, Token: tokenService.GenerateToken(new GenerateTokenRequest(user.Id))))
			.Map(tuple => new AuthenticationResponse(tuple.User.Id
				, tuple.User.EmailAddress
				, tuple.User.FirstName
				, tuple.User.LastName
				, tuple.User.CreatedAt
				, tuple.Token.Token
				, tuple.Token.ExpiresAt));
}
