using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class AdministratorAuthorizeAttribute : RoleAuthorizeAttribute
	{
		public AdministratorAuthorizeAttribute()
			: base(typeof(AdministratorRole))
		{
		}
	}
}