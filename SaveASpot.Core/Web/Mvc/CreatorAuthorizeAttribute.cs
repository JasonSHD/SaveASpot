using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class CreatorAuthorizeAttribute : RoleAuthorizeAttribute
	{
		public CreatorAuthorizeAttribute()
			: base(typeof(CreatorRole))
		{
		}
	}
}