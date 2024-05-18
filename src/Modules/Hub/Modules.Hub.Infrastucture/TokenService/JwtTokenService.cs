using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace Modules.Hub.Infrastucture.TokenService;

internal sealed class JwtTokenService(IDateTimeProvider dateTimeProvider, IOptions<JwtTokenServiceConfiguration> options) : ITokenService
{
	private readonly JwtTokenServiceConfiguration configuration = options.GetConfiguration();

	public GeneratedTokenResult GenerateToken(GenerateTokenRequest request)
	{
		var now = dateTimeProvider.UtcNow.UtcDateTime;

		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.SecretKey))
				, SecurityAlgorithms.HmacSha256Signature);

		var userIdString = request.UserId.Value.ToString();

		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim(JwtRegisteredClaimNames.Sub, userIdString),
			new Claim(ClaimNames.UserId, userIdString),
		};

		var header = new JwtHeader(signingCredentials);

		var expiresAt = now.Add(configuration.TokenLifetime);

		var payload = new JwtPayload(issuer: configuration.Issuer
			, audience: configuration.Audience
			, claims: claims
			, notBefore: now
			, expires: expiresAt
			, issuedAt: now);

		var token = new JwtSecurityToken(header, payload);
		var tokenHandler = new JwtSecurityTokenHandler();

		return new GeneratedTokenResult(tokenHandler.WriteToken(token)
			, new DateTimeOffset(expiresAt, TimeSpan.Zero));
	}
}
