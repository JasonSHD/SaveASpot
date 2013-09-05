namespace SaveASpot.Core.Web
{
	public interface IWebAuthentication
	{
		void Authenticate(IElementIdentity userIdentity, bool createPersistentCookie);
		void LogOff();
	}
}
