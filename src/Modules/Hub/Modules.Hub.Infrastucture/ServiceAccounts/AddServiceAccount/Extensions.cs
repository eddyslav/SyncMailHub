using System.Text;

namespace Modules.Hub.Infrastucture.ServiceAccounts.AddServiceAccount;

// TODO: move to common lib
internal static class Extensions
{
	private static Func<string, Error> SessionValueNotFound { get; } =
		sessionKeyName => new BadRequestError("ServiceAccount.AddFailed", $"\"{sessionKeyName}\" was not found in session storage");

	private static Func<string, Error> SessionValueInvalid { get; } =
		sessionKeyName => new BadRequestError("ServiceAccount.AddFailed", $"\"{sessionKeyName}\" does not contain valid Guid value");

	public static Result<ISession> GetSession(this IHttpContextAccessor httpContextAccessor) => Result.Create(httpContextAccessor.HttpContext?.Session);

	public static Result<string> GetStringValue(this Result<ISession> result, string key) =>
		result.Bind(session => session.TryGetValue(key, out var value)
			? Result.Success(Encoding.UTF8.GetString(value))
			: Result.Failure<string>(SessionValueNotFound(key)));

	public static Result<Guid> GetGuidValue(this Result<ISession> result, string key) =>
		result.GetStringValue(key)
			.Bind(stringValue => Guid.TryParse(stringValue, out var guidValue)
				? Result.Success(guidValue)
				: Result.Failure<Guid>(SessionValueInvalid(key)));

	public static Result<ISession> SetStringValue(this Result<ISession> result
		, string key
		, string value) =>
		result.Tap(session => session.Set(key, Encoding.UTF8.GetBytes(value)));

	public static Result<ISession> SetGuidValue(this Result<ISession> result
		, string key
		, Guid value) =>
		result.SetStringValue(key, value.ToString());
}
