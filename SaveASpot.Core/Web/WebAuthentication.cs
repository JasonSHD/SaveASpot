using System.Web.Security;

namespace SaveASpot.Core.Web
{
	public sealed class WebAuthentication : IWebAuthentication
	{
		public void Authenticate(IElementIdentity elementIdentity, bool createPersistentCookie)
		{
			FormsAuthentication.SetAuthCookie(elementIdentity.ToString(), createPersistentCookie);
		}

		public void LogOff()
		{
			FormsAuthentication.SignOut();
		}
	}
}