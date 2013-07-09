using System.Web.Security;

namespace SaveASpot.Core.Web
{
	public sealed class WebAuthentication : IWebAuthentication
	{
		public void Authenticate(string username, bool createPersistentCookie)
		{
			FormsAuthentication.SetAuthCookie(username, createPersistentCookie);
		}
	}
}