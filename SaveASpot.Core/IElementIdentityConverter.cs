namespace SaveASpot.Core
{
	public interface IElementIdentityConverter
	{
		IElementIdentity ToIdentity(string identity);
		IElementIdentity ToIdentity(object identity);
		IElementIdentity GenerateNew();
		bool IsEqual(IElementIdentity left, IElementIdentity right);
	}
}