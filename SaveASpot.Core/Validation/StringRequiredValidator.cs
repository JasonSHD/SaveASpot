namespace SaveASpot.Core.Validation
{
	public sealed class StringRequiredValidator : BaseValidator<string>
	{
		public StringRequiredValidator()
			: base("StringRequired")
		{
		}

		protected override bool IsValid(string input)
		{
			return !string.IsNullOrWhiteSpace(input);
		}
	}
}