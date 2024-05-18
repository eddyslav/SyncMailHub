using Modules.Hub.Application.Users.SignInUser;
using Modules.Hub.Application.Users.SignUpUser;
using Modules.Hub.Application.Users.CurrentUser;

using Modules.Hub.Presentation.Users.Contracts;

namespace Modules.Hub.Presentation.Users;

public sealed class UsersModule : ICarterModule
{
	private static async Task<IResult> HandleSignUpAsync(SignUpUserRequest request
		, ISender sender
		, CancellationToken cancellationToken) =>
		await Result.Create(new SignUpUserCommand(request.EmailAddress
			, request.FirstName
			, request.LastName
			, request.Password))
			.Bind(command => sender.Send(command, cancellationToken))
			.Match(Results.Ok);

	private static async Task<IResult> HandleSignInAsync(SignInUserRequest request
		, ISender sender
		, CancellationToken cancellationToken) =>
		await Result.Create(new SignInUserCommand(request.EmailAddress, request.Password))
			.Bind(command => sender.Send(command, cancellationToken))
			.Match(Results.Ok);

	private static async Task<IResult> HandlerCurrentUserAsync(ISender sender, CancellationToken cancellationToken) =>
		await Result.Success()
			.Bind(() => sender.Send(GetCurrentUserQuery.Instance, cancellationToken))
			.Match(Results.Ok);

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		var group = app
			.MapGroup("api/users");

		group.MapPost("sign-up", HandleSignUpAsync).AllowAnonymous();
		group.MapPost("sign-in", HandleSignInAsync).AllowAnonymous();

		group.MapGet("me", HandlerCurrentUserAsync)
			.RequireAuthorization();
	}
}
