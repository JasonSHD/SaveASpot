using System.Web.Mvc;
using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class CustomerActionFilter : IActionFilter
	{
		private readonly ICurrentUser _currentUser;
		private readonly ICurrentCustomer _currentCustomer;
		private readonly IRoleFactory _roleFactory;

		public CustomerActionFilter(ICurrentUser currentUser, ICurrentCustomer currentCustomer, IRoleFactory roleFactory)
		{
			_currentUser = currentUser;
			_currentCustomer = currentCustomer;
			_roleFactory = roleFactory;
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (_currentUser.User.IsCustomer(_roleFactory.Convert(typeof(CustomerRole))))
			{
				CustomerElement.SetCustomer(_currentCustomer.Customer, filterContext.Controller.ViewBag);
			}
		}
	}
}