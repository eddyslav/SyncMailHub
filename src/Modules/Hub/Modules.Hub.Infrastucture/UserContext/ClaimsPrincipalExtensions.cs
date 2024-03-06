using Modules.Hub.Domain.Users;

namespace Modules.Hub.Infrastucture.UserContext;

internal static class ClaimsPrincipalExtensions
{
	private static Result<string> GetClaimValue(this ClaimsPrincipal principal, string claimName) =>
		Result
			.Create(principal.FindFirst(claim => claim.Type == claimName))
			.MapFailure(() => new UnauthorizedError("Claim.NotFound", $"'{claimName}' claim cannot be found"))
			.Map(claim => claim.Value);

	private static Result<Guid> ParseAsGuid(string claimName, string claimValue) =>
		Result
			.Create(claimValue)
			.Bind(value => Guid.TryParse(value, out var guidValue)
				? Result.Success(guidValue)
				: Result.Failure<Guid>(new UnauthorizedError("Claim.InvalidValue", $"'{claimName}' cannot be parsed as Guid value")));

	private static Result<Guid> GetClaimGuidValue(this ClaimsPrincipal principal, string claimName) =>
		principal
			.GetClaimValue(claimName)
			.Bind(claimValue => ParseAsGuid(claimName, claimValue));

	public static Result<UserId> GetUserId(this ClaimsPrincipal principal) =>
		principal
			.GetClaimGuidValue(ClaimNames.UserId)
			.Map(guidValue => new UserId(guidValue));
}
