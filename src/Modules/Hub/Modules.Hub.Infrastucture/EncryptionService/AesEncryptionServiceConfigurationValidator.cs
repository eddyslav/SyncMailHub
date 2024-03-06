namespace Modules.Hub.Infrastucture.EncryptionService;

internal sealed class AesEncryptionServiceConfigurationValidator : AbstractValidator<AesEncryptionServiceConfiguration>
{
	public AesEncryptionServiceConfigurationValidator()
	{
		RuleFor(x => x.EncryptionKey).NotEmpty();
	}
}
