using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class CustomerAuthorizeAttribute : RoleAuthorizeAttribute
	{
		public CustomerAuthorizeAttribute()
			: base(typeof(CustomerRole))
		{
		}
	}
}