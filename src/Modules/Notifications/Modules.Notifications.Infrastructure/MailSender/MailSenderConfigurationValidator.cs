using static Modules.Notifications.Infrastructure.MailSender.MailSenderConfiguration;

namespace Modules.Notifications.Infrastructure.MailSender;

internal sealed class MailSenderConfigurationValidator : AbstractValidator<MailSenderConfiguration>
{
	private sealed class TemplateWithSubjectValidator : AbstractValidator<EmailTemplate>
	{
		public TemplateWithSubjectValidator()
		{
			RuleFor(x => x.Subject).NotEmpty();

			RuleFor(x => x.Template).NotEmpty();
		}
	}

	public MailSenderConfigurationValidator()
	{
		var templateValidator = new TemplateWithSubjectValidator();

		RuleFor(x => x.WelcomeTemplate)
			.SetValidator(templateValidator);
	}
}
