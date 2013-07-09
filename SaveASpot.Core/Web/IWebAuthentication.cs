namespace SaveASpot.Core.Web
{
	public interface IWebAuthentication
	{
		void Authenticate(string username, bool createPersistentCookie);
	}
}
