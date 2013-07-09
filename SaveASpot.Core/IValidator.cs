namespace SaveASpot.Core
{
	public interface IValidator<in T>
	{
		IValidationResult Validate(T input);
	}

	public static class ValidatorExtensions
	{
		public static IValidator<T> And<T>(this IValidator<T> validator, IValidator<T> and)
		{
			return new AndValidator<T>(validator, and);
		}
	}
}