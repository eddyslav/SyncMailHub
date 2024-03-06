namespace Modules.Hub.Infrastucture.PasswordHasherService;

internal sealed class BCryptPasswordHasherServiceConfigurationValidator : AbstractValidator<BCryptPasswordHasherServiceConfiguration>
{
	public BCryptPasswordHasherServiceConfigurationValidator()
	{
		RuleFor(x => x.WorkFactor)
			.InclusiveBetween(4, 31);

		RuleFor(x => x.HashType)
			.IsInEnum();
	}
}
