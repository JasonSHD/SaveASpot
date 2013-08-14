namespace SaveASpot.Core.Validation
{
	public static class NotValidatorBuilderExtensions
	{
		public static NotValidatorBuilder Not(this IValidator source)
		{
			return new NotValidatorBuilder();
		}

		public static IValidator IsEmpty(this NotValidatorBuilder source, IElementIdentityConverter elementIdentityConverter)
		{
			return source.Add(new RequiredElementIdentityValidator(elementIdentityConverter));
		}
	}
}