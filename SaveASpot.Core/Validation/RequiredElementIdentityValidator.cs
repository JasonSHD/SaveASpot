namespace SaveASpot.Core.Validation
{
	public sealed class RequiredElementIdentityValidator : IValidator
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public RequiredElementIdentityValidator(IElementIdentityConverter elementIdentityConverter)
		{
			_elementIdentityConverter = elementIdentityConverter;
		}

		public IValidationResult Validate(object input)
		{
			return new ValidationResult(!(_elementIdentityConverter.ToIdentity(input) is NullElementIdentity), string.Empty);
		}
	}
}