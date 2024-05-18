namespace Modules.Hub.Application.Users.CurrentUser;

public sealed record GetCurrentUserQuery : IQuery<CurrentUserResponse>
{
	private GetCurrentUserQuery()
	{

	}

	public static GetCurrentUserQuery Instance { get; } = new();
}
