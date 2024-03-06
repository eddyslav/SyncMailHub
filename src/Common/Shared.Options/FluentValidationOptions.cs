namespace Shared.Options;

internal sealed class FluentValidateOptions<TOptions> : IValidateOptions<TOptions>
	where TOptions : class
{
	private readonly string? name;
	private readonly IValidator<TOptions> validator;

	public FluentValidateOptions(string? name, IValidator<TOptions> validator) =>
		(this.name, this.validator) = (name, validator);

	public ValidateOptionsResult Validate(string? name, TOptions options)
	{
		if (this.name is not null && this.name != name)
		{
			return ValidateOptionsResult.Skip;
		}

		var validationResult = validator.Validate(options);
		if (validationResult.IsValid)
		{
			return ValidateOptionsResult.Success;
		}

		var errors = validationResult
			.Errors
			.Select(error => $"Options validation failed for \"{error.PropertyName}\" with error: {error.ErrorMessage}");

		return ValidateOptionsResult.Fail(errors);
	}
}
