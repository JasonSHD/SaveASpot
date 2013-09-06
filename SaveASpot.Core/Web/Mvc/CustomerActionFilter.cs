using System.Web.Mvc;
using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class CustomerActionFilter : IActionFilter
	{
		private readonly ICurrentUser _currentUser;
		private readonly ICurrentCustomer _currentCustomer;

		public CustomerActionFilter(ICurrentUser currentUser, ICurrentCustomer currentCustomer)
		{
			_currentUser = currentUser;
			_currentCustomer = currentCustomer;
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (_currentUser.User.IsCustomer())
			{
				CustomerElement.SetCustomer(_currentCustomer.Customer, filterContext.Controller.ViewBag);
			}
		}
	}
}