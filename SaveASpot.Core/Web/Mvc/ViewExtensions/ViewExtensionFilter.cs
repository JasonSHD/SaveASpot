using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc.ViewExtensions
{
	public sealed class ViewExtensionFilter : IActionFilter
	{
		private readonly IViewExtensionsBuilder _viewExtensionsBuilder;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public ViewExtensionFilter(IViewExtensionsBuilder viewExtensionsBuilder, IElementIdentityConverter elementIdentityConverter)
		{
			_viewExtensionsBuilder = viewExtensionsBuilder;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var viewResult = filterContext.Result as ViewResult;

			if (viewResult != null)
			{
				var extensions = _viewExtensionsBuilder.CollectExtensions(filterContext.RouteData.Values.Select(e => new KeyValuePair<string, string>(e.Key, (e.Value ?? string.Empty).ToString())));
				SetViewExtensionExecuter((Func<ViewContext, IViewExtensionExecuter>)(c => new ViewExtensionExecuter(extensions, c, _elementIdentityConverter)), viewResult.ViewBag);
			}
		}

		private static void SetViewExtensionExecuter(Func<ViewContext, IViewExtensionExecuter> viewExtensionExecuterBuilder, dynamic viewBag)
		{
			viewBag._______________viewExtensionExecuterBuilder = viewExtensionExecuterBuilder;
		}

		public static IViewExtensionExecuter GetViewExtensionExecuter(ViewContext viewContext)
		{
			var builder = (Func<ViewContext, IViewExtensionExecuter>)viewContext.Controller.ViewBag._______________viewExtensionExecuterBuilder;
			return builder(viewContext);
		}
	}
}
