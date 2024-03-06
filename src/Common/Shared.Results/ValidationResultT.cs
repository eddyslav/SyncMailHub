namespace Shared.Results;

public sealed class ValidationResult<T> : Result<T>, IValidationResult
	where T : notnull
{
	private ValidationResult(IEnumerable<Error> errors) : base(default, false, IValidationResult.ValidationError) =>
		Errors = errors;

	public IEnumerable<Error> Errors { get; }

	public static ValidationResult<T> WithErrors(IEnumerable<Error> errors) => new(errors);
}
