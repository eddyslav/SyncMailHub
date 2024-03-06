namespace Modules.Hub.Infrastucture.Google;

internal sealed class GoogleOAuthConfigurationValidator : AbstractValidator<GoogleOAuthConfiguration>
{
	public GoogleOAuthConfigurationValidator()
	{
		RuleFor(x => x.ClientId)
			.NotEmpty();

		RuleFor(x => x.ClientSecret)
			.NotEmpty();

		RuleFor(x => x.RedirectUri)
			.Must(x => x is { IsAbsoluteUri: true })
			.WithMessage("Must be a valid well formed absolute uri");
	}
}
