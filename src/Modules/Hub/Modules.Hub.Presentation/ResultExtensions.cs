using Shared.Results.Errors;

namespace Modules.Hub.Presentation;

// TODO: refactor this class into common project (if considering adding endpoints to other modules)
internal static class ResultExtensions
{
	private static readonly string validationErrorTitle = "Validation Failed";
	private static readonly string badRequestErrorTitle = "Bad Request";
	private static readonly string unauthorizedTitle = "Authorization Failed";
	private static readonly string notFoundTitle = "Not Found";
	private static readonly string conflictTitle = "Conflict";
	private static readonly string defaultTitle = "Internal Server Error";

	private static IResult CreateProblemResult(string title, Error error, int statusCode) =>
		Results.Problem(title: title
			, type: error.Code
			, detail: error.Message
			, statusCode: statusCode);

	private static IResult CreateValidationProblemResult(IValidationResult validationResult) =>
		Results.ValidationProblem(validationResult.Errors.ToDictionary(error => error.Code, error => new[] { error.Message })
			, title: validationErrorTitle
			, type: IValidationResult.ValidationError.Code
			, detail: IValidationResult.ValidationError.Message);

	private static (int, string) GetCodeAndTitle(Error error) =>
		error switch
		{
			BadRequestError => (StatusCodes.Status400BadRequest, badRequestErrorTitle),
			UnauthorizedError => (StatusCodes.Status401Unauthorized, unauthorizedTitle),
			NotFoundError => (StatusCodes.Status404NotFound, notFoundTitle),
			ConflictError => (StatusCodes.Status409Conflict, conflictTitle),
			ExternalServiceCallError => (StatusCodes.Status502BadGateway, defaultTitle),
			_ => (StatusCodes.Status500InternalServerError, defaultTitle),
		};

	private static IResult CreateGenericResponse(Error error)
	{
		var (statusCode, title) = GetCodeAndTitle(error);

		return CreateProblemResult(title, error, statusCode);
	}

	private static IResult HandleError(Result result) =>
		result switch
		{
			{ IsSuccess: true } => throw new InvalidOperationException("Cannot handle success result"),
			IValidationResult validationResult => CreateValidationProblemResult(validationResult),
			_ => CreateGenericResponse(result.Error),
		};

	public static async Task<IResult> Match<T>(this Task<Result<T>> resultTask
		, Func<T, IResult> onSuccess
		, Func<Result, IResult> onFailure)
		where T : notnull
	{
		var result = await resultTask;

		return result.IsSuccess
			? onSuccess(result.Value)
			: onFailure(result);
	}

	public static Task<IResult> Match<T>(this Task<Result<T>> resultTask, Func<T, IResult> onSuccess) where T : notnull =>
		resultTask.Match(onSuccess, HandleError);

	public static async Task<IResult> Match(this Task<Result> resultTask
		, Func<IResult> onSuccess
		, Func<Result, IResult> onFailure)
	{
		var result = await resultTask;

		return result.IsSuccess
			? onSuccess()
			: onFailure(result);
	}

	public static Task<IResult> Match(this Task<Result> resultTask, Func<IResult> onSuccess) =>
		resultTask.Match(onSuccess, HandleError);
}
