namespace SaveASpot.Core
{
	public sealed class AndValidator<T> : IValidator<T>
	{
		private readonly IValidator<T> _left;
		private readonly IValidator<T> _right;

		public AndValidator(IValidator<T> left, IValidator<T> right)
		{
			_left = left;
			_right = right;
		}

		public IValidationResult Validate(T input)
		{
			var validationResult = _left.Validate(input);
			if (!validationResult.IsValid) return validationResult;
			validationResult = _right.Validate(input);
			if (!validationResult.IsValid) return validationResult;

			return validationResult;
		}
	}
}