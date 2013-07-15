using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[TabDescriptions(SiteConstants.PhasesControllerAlias, new[] { typeof(AdministratorRole) }, "Phases")]
	public sealed class PhasesController : AdminTabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}
	}
}