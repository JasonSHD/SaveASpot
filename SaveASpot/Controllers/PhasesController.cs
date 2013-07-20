using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[TabDescriptions(SiteConstants.PhasesControllerAlias, "Phases")]
	[AdministratorAuthorize]
	public sealed class PhasesController : AdminTabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}
	}
}