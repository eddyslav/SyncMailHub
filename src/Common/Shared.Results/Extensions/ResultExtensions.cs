namespace Shared.Results.Extensions;

public static class ResultExtensions
{
	private static Result<T> MapToResultFailure<T>(Result result) where T : notnull =>
		result is Result<T> valueResult
			? valueResult
			: Result.Failure<T>(result.Error);

	public static Result<T> Map<T>(this Result result, Func<T> mapper) where T : notnull =>
		result.IsSuccess
			? mapper()
			: MapToResultFailure<T>(result);

	public static async Task<Result<T>> Map<T>(this Result result, Func<Task<T>> mapper) where T : notnull =>
		result.IsSuccess
			? await mapper()
			: MapToResultFailure<T>(result);

	public static async Task<Result<T>> Map<T>(this Task<Result> resultTask, Func<T> mapper) where T : notnull =>
		(await resultTask).Map(mapper);

	public static Result<T1> Map<T0, T1>(this Result<T0> result, Func<T0, T1> mapper)
		where T0 : notnull
		where T1 : notnull =>
		result.IsSuccess
			? mapper(result.Value)
			: Result.Failure<T1>(result.Error);

	public static async Task<Result<T1>> Map<T0, T1>(this Task<Result<T0>> resultTask, Func<T0, T1> mapper)
		where T0 : notnull
		where T1 : notnull =>
		(await resultTask).Map(mapper);

	public static async Task<Result<T1>> Map<T0, T1>(this Result<T0> result, Func<T0, Task<T1>> mapper)
		where T0 : notnull
		where T1 : notnull =>
		result.IsSuccess
			? await mapper(result.Value)
			: Result.Failure<T1>(result.Error);

	public static async Task<Result<T1>> Map<T0, T1>(this Task<Result<T0>> resultTask, Func<T0, Task<T1>> mapper)
		where T0 : notnull
		where T1 : notnull =>
		await (await resultTask).Map(mapper);

	public static Result MapFailure(this Result result, Func<Error> errorMapper) =>
		result.IsSuccess
			? result
			: Result.Failure(errorMapper());

	public static Result MapFailure(this Result result, Error error) =>
		result.IsSuccess
			? result
			: Result.Failure(error);

	public static Result<T> MapFailure<T>(this Result<T> result, Func<Error> errorMapper) where T : notnull =>
		result.IsSuccess
			? result
			: Result.Failure<T>(errorMapper());

	public static Result<T> MapFailure<T>(this Result<T> result, Error error) where T : notnull =>
		result.IsSuccess
			? result
			: Result.Failure<T>(error);

	public static Result<T> MapFailure<T>(this Result<T> result, Func<Error, Error> errorMapper) where T : notnull =>
		result.IsSuccess
			? result
			: Result.Failure<T>(errorMapper(result.Error));

	public static async Task<Result<T>> MapFailure<T>(this Task<Result<T>> resultTask, Error error) where T : notnull =>
		(await resultTask).MapFailure(error);

	public static async Task<Result<T>> MapFailure<T>(this Task<Result<T>> resultTask, Func<Error> errorMapper) where T : notnull =>
		(await resultTask).MapFailure(errorMapper);

	public static async Task<Result<T>> MapFailure<T>(this Task<Result<T>> resultTask, Func<Error, Error> errorMapper) where T : notnull =>
		(await resultTask).MapFailure(errorMapper);

	public static Result Bind<T>(this Result<T> result, Func<T, Result> func) where T : notnull =>
		result.IsSuccess
			? func(result.Value)
			: Result.Failure(result.Error);

	public static async Task<Result> Bind<T>(this Task<Result<T>> result, Func<T, Result> func) where T : notnull =>
		(await result).Bind(func);

	public static async Task<Result> Bind<T>(this Result<T> result, Func<T, Task<Result>> func) where T : notnull =>
		result.IsSuccess
			? await func(result.Value)
			: Result.Failure(result.Error);

	public static async Task<Result> Bind<T>(this Task<Result<T>> result, Func<T, Task<Result>> func) where T : notnull =>
		await (await result).Bind(func);

	public static Result<T1> Bind<T0, T1>(this Result<T0> result, Func<T0, Result<T1>> func)
		where T0 : notnull
		where T1 : notnull =>
		result.IsSuccess
			? func(result.Value)
			: Result.Failure<T1>(result.Error);

	public static async Task<Result<T1>> Bind<T0, T1>(this Result<T0> result, Func<T0, Task<Result<T1>>> func)
		where T0 : notnull
		where T1 : notnull =>
		result.IsSuccess
			? await func(result.Value)
			: Result.Failure<T1>(result.Error);

	public static async Task<Result<T>> Bind<T>(this Result result, Func<Task<Result<T>>> func) where T : notnull =>
		result.IsSuccess
			? await func()
			: Result.Failure<T>(result.Error);

	public static async Task<Result<T1>> Bind<T0, T1>(this Task<Result<T0>> resultTask, Func<T0, Result<T1>> func)
		where T0 : notnull
		where T1 : notnull =>
		(await resultTask).Bind(func);

	public static async Task<Result<T1>> Bind<T0, T1>(this Task<Result<T0>> resultTask, Func<T0, Task<Result<T1>>> func)
		where T0 : notnull
		where T1 : notnull =>
		await (await resultTask).Bind(func);

	public static async Task<Result> Tap(this Result result, Func<Task> func)
	{
		if (result.IsSuccess)
		{
			await func();
		}

		return result;
	}

	public static async Task<Result> Tap(this Task<Result> resultTask, Func<Task> func) => await (await resultTask).Tap(func);

	public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
		where T : notnull
	{
		if (result.IsSuccess)
		{
			action(result.Value);
		}

		return result;
	}

	public static async Task<Result<T>> Tap<T>(this Task<Result<T>> resultTask, Func<Task> func) where T : notnull =>
		await (await resultTask).Tap(func);

	public static async Task<Result<T>> Tap<T>(this Task<Result<T>> resultTask, Action<T> action) where T : notnull =>
		(await resultTask).Tap(action);

	public static async Task<Result<T>> Tap<T>(this Result<T> result, Func<Task> func)
		where T : notnull
	{
		if (result.IsSuccess)
		{
			await func();
		}

		return result;
	}

	public static async Task<Result> OnFailure(this Task<Result> resultTask, Action<Error> action)
	{
		var result = await resultTask;

		if (!result.IsSuccess)
		{
			action(result.Error);
		}

		return result;
	}

	public static async Task<Result<T>> OnFailure<T>(this Task<Result<T>> resultTask, Action<Error> action)
		where T : notnull
	{
		var result = await resultTask;

		if (!result.IsSuccess)
		{
			action(result.Error);
		}

		return result;
	}

	public static Result<T> Filter<T>(this Result<T> result, Func<T, bool> predicate)
		where T : notnull
	{
		return result.IsSuccess
			? predicate(result.Value)
				? result
				: Result.Failure<T>(Error.ConditionNotMet)
			: result;
	}
}
