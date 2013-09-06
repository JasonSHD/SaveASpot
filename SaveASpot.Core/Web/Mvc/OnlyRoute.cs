using System.Web.Routing;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class OnlyRoute : Route
	{
		public OnlyRoute(string url, IRouteHandler routeHandler)
			: base(url, routeHandler)
		{
		}

		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			return null;
		}
	}
}