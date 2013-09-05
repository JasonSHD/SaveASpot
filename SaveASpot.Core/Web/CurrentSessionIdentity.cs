using System;
using System.Linq;
using System.Web;

namespace SaveASpot.Core.Web
{
	public sealed class CurrentSessionIdentity : ICurrentSessionIdentity
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;
		private const string CurrentSessionCookiesIdentityKey = "__CurrentSessionIdentity__";

		public CurrentSessionIdentity(IElementIdentityConverter elementIdentityConverter)
		{
			_elementIdentityConverter = elementIdentityConverter;
		}

		public IElementIdentity UserIdentity
		{
			get { return _elementIdentityConverter.ToIdentity(HttpContext.Current.User.Identity.Name); }
		}

		public IElementIdentity SessionIdentity
		{
			get
			{
				if (HttpContext.Current.Request.Cookies.AllKeys.Any(
					e => CurrentSessionCookiesIdentityKey.Equals((string) e, StringComparison.CurrentCultureIgnoreCase)))
				{
					var sessionCookieIdentity = DecryptString(HttpContext.Current.Request.Cookies[CurrentSessionCookiesIdentityKey].Value);
					return _elementIdentityConverter.ToIdentity(sessionCookieIdentity);
				}

				var newSessionCookieIdentity = _elementIdentityConverter.GenerateNew();

				var newSessionCookieValue = EncryptString(newSessionCookieIdentity.ToString());
				HttpContext.Current.Request.Cookies.Add(new HttpCookie(CurrentSessionCookiesIdentityKey, newSessionCookieValue));
				HttpContext.Current.Response.Cookies.Add(new HttpCookie(CurrentSessionCookiesIdentityKey, newSessionCookieValue));

				return newSessionCookieIdentity;
			}
		}

		private string EncryptString(string input)
		{
			return input;
		}

		private string DecryptString(string input)
		{
			return input;
		}
	}
}