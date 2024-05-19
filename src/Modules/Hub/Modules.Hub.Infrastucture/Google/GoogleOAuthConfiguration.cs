namespace Modules.Hub.Infrastucture.Google;

internal sealed class GoogleOAuthConfiguration
{
	private static IReadOnlyCollection<string> scopes =
	[
		"https://mail.google.com/",
		"openid email",
	];

	public IReadOnlyCollection<string> Scopes => scopes;

	public required string ClientId { get; init; }

	public required string ClientSecret { get; init; }

	public required Uri RedirectUri { get; init; }
}
