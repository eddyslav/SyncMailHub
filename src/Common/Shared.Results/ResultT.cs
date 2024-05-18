namespace Shared.Results;

public class Result<T> : Result
	where T : notnull
{
	private readonly T? value;

	protected internal Result(T? value, bool isSuccess, Error error)
		: base(isSuccess, error)
	{
		if (isSuccess && value is null)
		{
			throw new InvalidOperationException();
		}

		this.value = value;
	}

	public T Value =>
		IsSuccess
			? value!
			: throw new InvalidOperationException("The value of a failed result can not be accessed");

	public static implicit operator Result<T>(T? value) => Create(value);
}
