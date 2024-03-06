using Serilog;
using Serilog.Context;

namespace Modules.Hub.Application.Behaviors;

public sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(ILogger logger) : IPipelineBehavior<TRequest, TResponse>
	where TRequest : class
	where TResponse : Result
{
	private readonly ILogger logger = logger.ForContext<RequestLoggingPipelineBehavior<TRequest, TResponse>>();

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;

		logger.Information("Processing request {RequestName}", requestName);

		var result = await next();

		if (result.IsSuccess)
		{
			logger.Information("Completed request {RequestName}", requestName);
		}
		else
		{
			using (LogContext.PushProperty("Error", result.Error, true))
			{
				IDisposable? validationDisposable = result is IValidationResult validationResult
					? LogContext.PushProperty("ValidationErrors", validationResult.Errors)
					: null;

				logger.Error("Completed request {RequestName} with error", requestName);

				validationDisposable?.Dispose();
			}
		}

		return result;
	}
}
