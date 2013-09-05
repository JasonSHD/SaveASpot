namespace SaveASpot.Core.Web
{
	public interface ICurrentSessionIdentity
	{
		IElementIdentity UserIdentity { get; }
		IElementIdentity SessionIdentity { get; }
	}
}