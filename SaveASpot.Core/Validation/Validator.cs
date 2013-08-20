namespace SaveASpot.Core.Validation
{
	public static class Validator
	{
		public static IValidator Build()
		{
			return new NullValidator();
		}

		public static EntityValidatorBuilder<T> For<T>()
		{
			return new EntityValidatorBuilder<T>();
		}
	}
}