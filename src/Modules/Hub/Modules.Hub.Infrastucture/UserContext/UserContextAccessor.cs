namespace Modules.Hub.Infrastucture.UserContext;

internal sealed class UserContextAccessor(IHttpContextAccessor httpContextAccessor) : IUserContextAccessor
{
	private static HttpContext GetContext(IHttpContextAccessor httpContextAccessor) =>
		httpContextAccessor.HttpContext
			?? throw new InvalidOperationException("Context was not set yet");

	private static UserId GetUserIdImpl(IHttpContextAccessor httpContextAccessor)
	{
		var httpContext = GetContext(httpContextAccessor);

		var userIdResult = httpContext.User.GetUserId();

		return userIdResult.IsSuccess
			? userIdResult.Value
			: throw new InvalidOperationException(userIdResult.Error.Message);
	}

	private readonly Lazy<UserId> userIdLazy = new(() => GetUserIdImpl(httpContextAccessor));

	public UserId UserId => userIdLazy.Value;
}
