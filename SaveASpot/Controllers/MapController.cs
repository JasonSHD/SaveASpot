using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[TabDescriptions(SiteConstants.MapControllerAlias, "Map")]
	[AdministratorAuthorize]
	[ClientAuthorize]
	public sealed class MapController : AdminTabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}
	}
}