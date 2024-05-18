using Serilog.Context;

namespace App.Middlewares;

internal sealed class CorrelationIdMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
		{
			await next(context);
		}
	}
}
