namespace SaveASpot.Core.Validation
{
	public sealed class RequiredValidator : BaseValidator<object>
	{
		private const string DefaultInvalidMessageKey = "ObjectRequired";

		public RequiredValidator() : base(DefaultInvalidMessageKey) { }

		protected override bool IsValid(object input)
		{
			return !ReferenceEquals(null, input);
		}
	}
}