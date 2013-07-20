using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[MainMenuTab(Alias = SiteConstants.MapControllerAlias, Area = "", IndexOfOrder = 10, Title = "MapTabTitle")]
	[CustomerAuthorize]
<<<<<<< HEAD
	[AdministratorAuthorize]
=======
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937
	public sealed class MapController : TabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}
	}
}