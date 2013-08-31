using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class AnonymAuthorizeAttribute : RoleAuthorizeAttribute
	{
		public AnonymAuthorizeAttribute()
			: base(typeof(AnonymRole))
		{
		}
	}
}