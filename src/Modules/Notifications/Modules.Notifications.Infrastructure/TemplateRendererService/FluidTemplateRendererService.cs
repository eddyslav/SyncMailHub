using System.Text;

using System.Security.Cryptography;

using System.Collections.Concurrent;

using Fluid;

namespace Modules.Notifications.Infrastructure.TemplateRenderService;

internal sealed class FluidTemplateRendererService : ITemplateRendererService
{
	private static readonly FluidParser parser = new();

	private static string ComputeTemplateKey(string template)
	{
		var bytes = Encoding.UTF8.GetBytes(template);

		var hashBytes = MD5.HashData(bytes);

		return hashBytes.Aggregate(new StringBuilder()
			, (builder, currentByte) => builder.Append(currentByte.ToString("X2")))
			.ToString();
	}

	private static IFluidTemplate CreateFluidTemplate(string template) =>
		parser.TryParse(template, out var fluidTemplate, out var errorMessage)
			? fluidTemplate
			: throw new InvalidOperationException($"Fluid template could not be created using provided template: {errorMessage}");

	private readonly ConcurrentDictionary<string, IFluidTemplate> templatesCache = [];

	public async Task<string> RenderTemplateAsync(string template
		, object model
		, CancellationToken cancellationToken)
	{
		var templateKey = ComputeTemplateKey(template);
		var fluidTemplate = templatesCache.GetOrAdd(templateKey, _ => CreateFluidTemplate(template));

		cancellationToken.ThrowIfCancellationRequested();
		return await fluidTemplate.RenderAsync(new TemplateContext(model));
	}
}
