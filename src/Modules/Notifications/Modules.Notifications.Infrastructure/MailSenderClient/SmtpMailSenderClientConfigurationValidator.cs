namespace Modules.Notifications.Infrastructure.EmailSenderClient;

internal sealed class SmtpMailSenderClientConfigurationValidator : AbstractValidator<SmtpMailSenderClientConfiguration>
{
	public SmtpMailSenderClientConfigurationValidator()
	{
		RuleFor(x => x.FromName).NotEmpty();

		RuleFor(x => x.FromEmailAddress).NotEmpty();

		RuleFor(x => x.Host).NotEmpty();

		RuleFor(x => x.Port).GreaterThanOrEqualTo(0);

		RuleFor(x => x.UserName).NotEmpty();

		RuleFor(x => x.Password).NotEmpty();
	}
}
