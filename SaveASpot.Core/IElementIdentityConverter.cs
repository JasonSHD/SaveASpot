namespace SaveASpot.Core
{
	public interface IElementIdentityConverter
	{
		IElementIdentity ToIdentity(string identity);
		IElementIdentity ToIdentity(object identity);
		bool IsEqual(IElementIdentity left, IElementIdentity right);
	}
}