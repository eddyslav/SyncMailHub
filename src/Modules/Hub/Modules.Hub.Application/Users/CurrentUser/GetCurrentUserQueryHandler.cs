namespace Modules.Hub.Application.Users.CurrentUser;

internal sealed class GetCurrentUserQueryHandler(IUserRepository userRepository, IUserContextAccessor userContext) : IQueryHandler<GetCurrentUserQuery, CurrentUserResponse>
{
	private async Task<Result<CurrentUserResponse>> GetUserByIdAsync(UserId userId, CancellationToken cancellationToken) =>
		Result.Create(await userRepository.GetByIdAsync(userId, cancellationToken))
			.Map(user => new CurrentUserResponse(user.Id
				, user.EmailAddress
				, user.FirstName
				, user.LastName
				, user.CreatedAt));

	public async Task<Result<CurrentUserResponse>> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken) =>
		await Result.Create(query)
			.Bind(() => GetUserByIdAsync(userContext.UserId, cancellationToken))
			.MapFailure(UserErrors.CurrentUserDoesNotExist);
}
