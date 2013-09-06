using System.Web;
using System.Web.Routing;
using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class AdminConstraint : IRouteConstraint
	{
		private readonly ICurrentUser _currentUser;

		public AdminConstraint(ICurrentUser currentUser)
		{
			_currentUser = currentUser;
		}

		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
											RouteDirection routeDirection)
		{
			return _currentUser.User.IsAdmin() || _currentUser.User.IsAnonym();
		}
	}
}