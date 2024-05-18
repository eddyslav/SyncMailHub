namespace Modules.Hub.Infrastucture.TokenService;

internal sealed class JwtTokenServiceConfiguration
{
	public required string SecretKey { get; init; }

	public required string Issuer { get; init; }

	public required string Audience { get; init; }

	public required TimeSpan TokenLifetime { get; init; }
}
