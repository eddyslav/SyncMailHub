namespace Modules.Hub.Infrastucture.TokenService;

internal sealed class JwtTokenServiceConfigurationValidator : AbstractValidator<JwtTokenServiceConfiguration>
{
	public JwtTokenServiceConfigurationValidator()
	{
		RuleFor(x => x.SecretKey)
			.NotEmpty();

		RuleFor(x => x.Issuer)
			.NotEmpty();

		RuleFor(x => x.Audience)
			.NotEmpty();

		RuleFor(x => x.TokenLifetime)
			.GreaterThan(TimeSpan.Zero);
	}
}
