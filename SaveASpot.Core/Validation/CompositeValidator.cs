namespace SaveASpot.Core.Validation
{
	public abstract class CompositeValidator : IValidator
	{
		private readonly IValidator _validator;

		protected CompositeValidator(IValidator validator)
		{
			_validator = validator;
		}

		public IValidationResult Validate(object input)
		{
			return _validator.Validate(input);
		}

		#region sealed members
		public override sealed bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override sealed int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override sealed string ToString()
		{
			return base.ToString();
		}
		#endregion sealed members
	}
}