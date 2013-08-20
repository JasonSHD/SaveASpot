namespace SaveASpot.Core.Validation
{
	public sealed class StringLenghtValidator : BaseValidator<string>
	{
		private readonly int _minValue;
		private readonly int _maxValue;
		public const int NotSettedValue = -100;

		public StringLenghtValidator(int minValue, int maxValue = NotSettedValue)
			: base(string.Format("StringHasInvalidLenght"))
		{
			_minValue = minValue;
			_maxValue = maxValue;
		}

		protected override bool IsValid(string input)
		{
			return !string.IsNullOrWhiteSpace(input) &&
						 input.Length > -_minValue &&
						 (_maxValue == NotSettedValue || input.Length <= _maxValue);
		}
	}
}