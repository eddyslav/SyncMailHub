namespace Shared.Results;

public sealed class ValidationResult : Result, IValidationResult
{
	private ValidationResult(IEnumerable<Error> errors) : base(false, IValidationResult.ValidationError) =>
		Errors = errors;

	public IEnumerable<Error> Errors { get; }

	public static ValidationResult WithErrors(IEnumerable<Error> errors) => new(errors);
}
