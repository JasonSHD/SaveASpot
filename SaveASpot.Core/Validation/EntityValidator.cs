namespace SaveASpot.Core.Validation
{
	public sealed class EntityValidator<T> : IValidator
	{
		private readonly IValidator _validator;

		public EntityValidator(IValidator validator)
		{
			_validator = validator;
		}

		public IValidationResult Validate(object input)
		{
			if (input is T)
			{
				return _validator.Validate((T)input);
			}

			return new ValidationResult(false, "IncorrectObjectType");
		}
	}
}