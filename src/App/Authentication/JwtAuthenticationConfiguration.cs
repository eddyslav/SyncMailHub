namespace App.Authentication;

internal sealed class JwtAuthenticationConfiguration
{
	public required string Issuer { get; init; }

	public required string Audience { get; init; }

	public required string SecretKey { get; init; }
}
