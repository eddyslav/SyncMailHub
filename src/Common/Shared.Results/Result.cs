namespace Shared.Results;

public class Result
{
	protected internal Result(bool isSuccess, Error error)
	{
		ArgumentNullException.ThrowIfNull(error);

		if (isSuccess && error != Error.None)
		{
			throw new InvalidOperationException();
		}

		if (!isSuccess && error == Error.None)
		{
			throw new InvalidOperationException();
		}

		IsSuccess = isSuccess;
		Error = error;
	}

	public bool IsSuccess { get; }

	public Error Error { get; }

	public static Result Success() => new(true, Error.None);

	public static Result Failure(Error error) => new(false, error);

	public static Result Create(bool condition) =>
		condition
			? Success()
			: Failure(Error.ConditionNotMet);

	public static Result<T> Success<T>(T value) where T : notnull => new(value, true, Error.None);

	public static Result<T> Failure<T>(Error error) where T : notnull => new(default, false, error);

	public static Result<T> Create<T>(T? value) where T : notnull =>
		value is not null
			? Success(value)
			: Failure<T>(Error.NullValue);

	public static Result<T> Create<T>(Nullable<T> nullableValue) where T : struct =>
		nullableValue.HasValue
			? Success(nullableValue.Value)
			: Failure<T>(Error.NullValue);
}
