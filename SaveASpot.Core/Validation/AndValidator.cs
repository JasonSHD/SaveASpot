namespace SaveASpot.Core.Validation
{
	public sealed class AndValidator : IValidator
	{
		private readonly IValidator _left;
		private readonly IValidator _right;

		public AndValidator(IValidator left, IValidator right)
		{
			_left = left;
			_right = right;
		}

		public IValidationResult Validate(object input)
		{
			var leftValidationResult = _left.Validate(input);
			if (!leftValidationResult.IsValid)
			{
				return leftValidationResult;
			}
			var rightValidationResult = _right.Validate(input);

			return rightValidationResult;
		}
	}
}