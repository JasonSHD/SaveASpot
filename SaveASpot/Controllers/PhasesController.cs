using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[TabDescriptions(SiteConstants.PhasesControllerAlias, "PhasesTabTitle", IndexOfOrder = 20)]
	[AdministratorAuthorize]
	public sealed class PhasesController : AdminTabController
	{
		private readonly IPhasesControllerService _phasesControllerService;

		public PhasesController(IPhasesControllerService phasesControllerService)
		{
			_phasesControllerService = phasesControllerService;
		}

		public ViewResult Index()
		{
			return TabView(new { });
		}
	}
}