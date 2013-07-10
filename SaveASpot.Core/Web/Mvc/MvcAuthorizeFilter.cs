using System.Linq;
using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class MvcAuthorizeFilter : FilterAttribute, IAuthorizationFilter
	{
		private readonly IAuthorizeManager _authorizeManager;

		public MvcAuthorizeFilter(IAuthorizeManager authorizeManager)
		{
			_authorizeManager = authorizeManager;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			var actionAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(RoleAuthorizeAttribute), true).Cast<RoleAuthorizeAttribute>();
			var controllerAttributes = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(RoleAuthorizeAttribute), true).Cast<RoleAuthorizeAttribute>();
			var attributes = actionAttributes.Union(controllerAttributes);

			var authorizeResult = _authorizeManager.AllowAccess(attributes.Select(e => e.RoleType));
			if (!authorizeResult.IsAllow)
			{
				filterContext.Result = new RedirectResult("~/");
			}
		}
	}
}