using FluentValidation;

namespace Shared.Results.FluentValidation;

public static class RuleBuilderOptionsExtensions
{
	public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Error error) =>
		rule
			.WithErrorCode(error.Code)
			.WithMessage(error.Message);
}