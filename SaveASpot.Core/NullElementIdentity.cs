namespace SaveASpot.Core
{
	public sealed class NullElementIdentity : IElementIdentity
	{
		string IElementIdentity.ToString()
		{
			return string.Empty;
		}
	}
}