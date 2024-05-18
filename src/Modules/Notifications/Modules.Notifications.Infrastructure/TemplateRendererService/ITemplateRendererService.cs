namespace Modules.Notifications.Infrastructure.TemplateRenderService;

internal interface ITemplateRendererService
{
	Task<string> RenderTemplateAsync(string template
		, object model
		, CancellationToken cancellationToken);
}
