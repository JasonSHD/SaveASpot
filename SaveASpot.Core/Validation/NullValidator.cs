namespace SaveASpot.Core.Validation
{
	public sealed class NullValidator : IValidator
	{
		public IValidationResult Validate(object input)
		{
			return new ValidationResult(true, string.Empty);
		}
	}
}