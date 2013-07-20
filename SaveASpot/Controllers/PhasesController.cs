using System.Linq;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[PhasePageTab(Alias = SiteConstants.PhasesControllerAlias, IndexOfOrder = 10, Title = "PhasesAccordionGroupTitle")]
	[MainMenuTab(Alias = SiteConstants.PhasesAndParcelsControllerAlias, Area = "", IndexOfOrder = 20, Title = "PhasesAndParcelsTabTitle")]
	[AdministratorAuthorize]
	public sealed class PhasesController : TabController
	{
		public ActionResult Index()
		{
			if (Request.Headers.AllKeys.Any(e => e == SiteConstants.MainMenuTabSpecificReadyAlias))
			{
				return View(null, SiteConstants.Layouts.ParcelsAndSpotsAjaxLayout);
			}

			return TabView(new { });
		}
	}
}