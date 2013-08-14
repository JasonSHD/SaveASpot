namespace SaveASpot.Core.Validation
{
	public static class FluentValidatorExtensions
	{
		public static IValidator And(this IValidator source, IValidator and)
		{
			return new AndValidator(source, and);
		}

		public static IValidator Required(this IValidator source)
		{
			return source.And(new RequiredValidator());
		}

		public static IValidator Regex(this IValidator source, string regex, string message = null)
		{
			return source.And(new RegexValidator(regex) { Message = message });
		}

		public static IValidator StringRequired(this IValidator source)
		{
			return source.And(new StringRequiredValidator());
		}

		public static IValidator Lenght(this IValidator source, int minValue, int maxValue = StringLenghtValidator.NotSettedValue, string message = null)
		{
			return source.And(new StringLenghtValidator(minValue, maxValue) { Message = message });
		}

		public static IValidator IsEmpty(this IValidator source, IElementIdentityConverter elementIdentityConverter)
		{
			return new RequiredValidator();
		}
	}
}