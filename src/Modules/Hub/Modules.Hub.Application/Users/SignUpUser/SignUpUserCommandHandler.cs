namespace Modules.Hub.Application.Users.SignUpUser;

internal sealed class SignUpUserCommandHandler(IUserRepository userRepository
	, IPasswordHasherService passwordHasher
	, IUnitOfWork unitOfWork
	, ITokenService tokenService) : ICommandHandler<SignUpUserCommand, AuthenticationResponse>
{
	private async Task<Result<SignUpUserCommand>> CheckIfUserByEmailAddressExistsAsync(SignUpUserCommand command, CancellationToken cancellationToken) =>
		Result.Create(!await userRepository.CheckIfExistsByEmailAddressAsync(command.EmailAddress, cancellationToken))
			.Map(() => command)
			.MapFailure(UserErrors.EmailAddressAlreadyTaken);

	public async Task<Result<AuthenticationResponse>> Handle(SignUpUserCommand command, CancellationToken cancellationToken) =>
		await Result.Create(command)
			.Bind(command => CheckIfUserByEmailAddressExistsAsync(command, cancellationToken))
			.Map(command => User.Create(command.EmailAddress
				, command.FirstName
				, command.LastName
				, passwordHasher.HashPassword(command.Password)))
			.Tap(userRepository.Add)
			.Tap(() => unitOfWork.SaveChangesAsync(cancellationToken))
			.Map(user => (User: user, Token: tokenService.GenerateToken(new GenerateTokenRequest(user.Id))))
			.Map(tuple => new AuthenticationResponse(tuple.User.Id
				, tuple.User.EmailAddress
				, tuple.User.FirstName
				, tuple.User.LastName
				, tuple.User.CreatedAt
				, tuple.Token.Token
				, tuple.Token.ExpiresAt));
}
