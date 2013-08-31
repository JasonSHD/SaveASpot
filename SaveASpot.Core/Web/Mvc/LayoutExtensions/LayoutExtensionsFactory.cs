using System.Collections.Generic;
using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc.LayoutExtensions
{
	public interface ILayoutExtensionsFactory
	{

	}

	public interface ILayoutExtension
	{
		IEnumerable<LayoutExtension> GetExtensions();
	}

	public sealed class LayoutExtension
	{
		public string Identity { get; set; }
		public string ViewName { get; set; }
		public object Model { get; set; }
	}

	public sealed class LayoutExtensions : ILayoutExtensionsFactory
	{
	}

	public sealed class LayoutExtensionsFilter : IActionFilter
	{
		private readonly ILayoutExtensionsFactory _layoutExtensions;

		public LayoutExtensionsFilter(ILayoutExtensionsFactory layoutExtensions)
		{
			_layoutExtensions = layoutExtensions;
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.Result is ViewResult)
			{

			}
		}
	}
}
