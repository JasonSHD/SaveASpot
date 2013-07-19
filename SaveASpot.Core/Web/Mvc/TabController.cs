using System.Linq;
using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc
{
	public abstract class TabController : BaseController
	{
		protected virtual ViewResult TabView(object model)
		{
			var tabAttributes = GetType().GetCustomAttributes(false).OfType<TabAttribute>().ToList();
			var tabDataAttribute = tabAttributes.SelectMany(e => e.GetType().GetCustomAttributes(false)).OfType<TabDataAttribute>().FirstOrDefault();
			var masterView = tabDataAttribute == null ? null : tabDataAttribute.MasterPath;

			if (Request.Headers.AllKeys.Any(e => tabAttributes.Select(atr => atr.GetType().Name).Contains(e)))
			{
				masterView = null;
			}

			var view = View(null, masterView, model);

			return view;
		}
	}
}