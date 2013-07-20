using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[PhasePageTab(Alias = "phasesAccordionGroup", IndexOfOrder = 10, Title = "PhasesAccordionGroupTitle")]
	[MainMenuTab(Alias = SiteConstants.PhasesAndParcelsControllerAlias, Area = "", IndexOfOrder = 20, Title = "PhasesAndParcelsTabTitle")]
	[AdministratorAuthorize]
	public sealed class PhasesController : TabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}
	}
}