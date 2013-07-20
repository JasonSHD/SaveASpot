using System.Linq;
using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc
{
	public abstract class TabController : BaseController
	{
		protected virtual ViewResult TabView(object model)
		{
			return TabView(null, model);
		}

		protected virtual ViewResult TabView(string viewName, object model)
		{
			var tabAttributes = GetType().GetCustomAttributes(false).OfType<TabAttribute>().ToList();
			var tabDataAttribute = tabAttributes.SelectMany(e => e.GetType().GetCustomAttributes(false)).OfType<TabDataAttribute>().FirstOrDefault();
			var masterView = tabDataAttribute == null ? null : tabDataAttribute.MasterPath;

			if (Request.Headers.AllKeys.Any(e => tabAttributes.Select(atr => atr.GetType().Name).Contains(e)))
			{
				masterView = tabDataAttribute == null ? null : tabDataAttribute.AjaxMaterPath;
			}

			var view = View(viewName, masterView, model);

			return view;
		}
	}
}