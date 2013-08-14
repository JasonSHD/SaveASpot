namespace SaveASpot.Core
{
	public interface IValidator
	{
		IValidationResult Validate(object input);
	}
}