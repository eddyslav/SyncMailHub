namespace Modules.Hub.Application.Behaviors;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	where TResponse : Result
{
	private static class ValidationResultFactory
	{
		private static readonly Type ResultType = typeof(Result);
		private static readonly Type ValidationResultGenericTypeDefinition = typeof(ValidationResult<>).GetGenericTypeDefinition();

		private static TResult CreateGenericValidationResult<TResult>(Type resultGenericType, IEnumerable<Error> errors) =>
			(TResult)ValidationResultGenericTypeDefinition
				.MakeGenericType(resultGenericType)
				.GetMethod(nameof(ValidationResult.WithErrors))!
				.Invoke(null, [errors])!;

		public static TResult Create<TResult>(IEnumerable<Error> errors)
			where TResult : Result =>
			typeof(TResult) == ResultType
				? (ValidationResult.WithErrors(errors) as TResult)!
				: CreateGenericValidationResult<TResult>(typeof(TResult).GenericTypeArguments[0], errors);
	}

	private readonly IReadOnlyCollection<IValidator<TRequest>> validators = validators.ToList();

	private async Task<TResponse?> ValidateAsync(TRequest request, CancellationToken cancellationToken)
	{
		var validationTasks = validators
			.Select(validator => validator.ValidateAsync(request, cancellationToken));

		var validationResults = await Task.WhenAll(validationTasks);

		var errors = validationResults
			.SelectMany(validationResult => validationResult.Errors, (_, failure) => new Error(failure.ErrorCode, failure.ErrorMessage))
			.ToList();

		return errors.Any()
			? ValidationResultFactory.Create<TResponse>(errors)
			: null;
	}

	public async Task<TResponse> Handle(TRequest request
		, RequestHandlerDelegate<TResponse> next
		, CancellationToken cancellationToken)
	{
		return validators.Any() && await ValidateAsync(request, cancellationToken) is { } validationError
			? validationError
			: await next();
	}
}
