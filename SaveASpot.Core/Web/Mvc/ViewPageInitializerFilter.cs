using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class ViewPageInitializerFilter : FilterAttribute, IActionFilter
	{
		private readonly ViewPageInitializer _viewPageInitializer;

		public ViewPageInitializerFilter(ViewPageInitializer viewPageInitializer)
		{
			_viewPageInitializer = viewPageInitializer;
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			ViewPageInitializer.SetInitializer(filterContext.Controller.ViewBag, _viewPageInitializer);
		}
	}
}