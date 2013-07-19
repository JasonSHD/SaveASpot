using System;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[MainMenuTab(Alias = SiteConstants.MapControllerAlias, Area = "", IndexOfOrder = 10, Title = "MapTabTitle")]
	[CustomerAuthorize]
	public sealed class MapController : AdminTabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class MainMenuTabAttribute : TabAttribute
	{
		public MainMenuTabAttribute()
			: base(typeof(MainMenuTabActionAttribute))
		{
		}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class MainMenuTabActionAttribute : DefaultTabActionAttribute { }
}