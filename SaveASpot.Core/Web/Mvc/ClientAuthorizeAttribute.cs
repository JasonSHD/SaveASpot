using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class ClientAuthorizeAttribute : RoleAuthorizeAttribute
	{
		public ClientAuthorizeAttribute()
			: base(typeof(ClientRole))
		{
		}
	}
}