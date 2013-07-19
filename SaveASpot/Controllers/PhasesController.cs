using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[AdministratorAuthorize]
	[PhasePageTab(Alias = "phaseAccordionGroup", IndexOfOrder = 10, Title = "PhasesAccordionGroupTitle")]
	public sealed class PhasesController : TabController
	{
		public ViewResult Index()
		{
			return View();
		}
	}
}