namespace SaveASpot.Core
{
	public static class ElementIdentityExtensions
	{
		public static bool IsNull(this IElementIdentity source)
		{
			return source is NullElementIdentity;
		}
	}
}