using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[MainMenuTab(Alias = SiteConstants.MapControllerAlias, Area = "", IndexOfOrder = 10, Title = "MapTabTitle")]
	[CustomerAuthorize]
	[AdministratorAuthorize]
	public sealed class MapController : TabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}
	}
}