using System.Web.Mvc;
using System.Web.Routing;

namespace SaveASpot.Core.Web.Mvc
{
	public static class RouteCollectionExtensions
	{
		public static void OnlyRoute(this RouteCollection source, string name, string url, object defaults)
		{
			var route = new OnlyRoute(url, new MvcRouteHandler())
										{
											Defaults = new RouteValueDictionary(defaults),
											Constraints = new RouteValueDictionary(new { }),
											DataTokens = new RouteValueDictionary()
										};

			source.Add(route);
		}
	}
}