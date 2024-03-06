using Modules.Notifications.Application.MailSender;

namespace Modules.Notifications.Infrastructure.MailSender;

internal sealed class MailSender(ITemplateRendererService templateRenderService
	, IMailSenderClient emailSenderClient
	, IOptions<MailSenderConfiguration> options) : IMailSender
{
	private readonly MailSenderConfiguration configuration = options.GetConfiguration();

	public async Task SendWelcomeAsync(SendWelcomeRequest request, CancellationToken cancellationToken)
	{
		var model = new
		{
			request.FirstName,
		};

		var templateConfiguration = configuration.WelcomeTemplate;
		var renderedBody = await templateRenderService.RenderTemplateAsync(templateConfiguration.Template, model, cancellationToken);

		var emailRequest = new SendEmailRequest(templateConfiguration.Subject
			, renderedBody
			, [request.EmailAddress]);

		await emailSenderClient.SendEmailAsync(emailRequest, cancellationToken);
	}

	public async Task SendAccountConnectedAsync(SendAccountConnectedRequest request, CancellationToken cancellationToken)
	{
		var model = new
		{
			request.FirstName,
			request.EmailAddress,
		};

		var templateConfiguration = configuration.ServiceAccountAddedTemplate;
		var renderedBody = await templateRenderService.RenderTemplateAsync(templateConfiguration.Template, model, cancellationToken);

		var emailRequest = new SendEmailRequest(templateConfiguration.Subject
			, renderedBody
			, [request.EmailAddress]);

		await emailSenderClient.SendEmailAsync(emailRequest, cancellationToken);
	}
}
