using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[MainMenuTab(Alias = SiteConstants.PhasesAndParcelsControllerAlias, Area = "", IndexOfOrder = 20, Title = "PhasesAndParcelsTabTitle")]
	[AdministratorAuthorize]
	public sealed class PhasesAndParcelsController : TabController
	{
		private readonly IPhasesAndParcelsControllerService _phasesControllerService;

		public PhasesAndParcelsController(IPhasesAndParcelsControllerService phasesControllerService)
		{
			_phasesControllerService = phasesControllerService;
		}

		public ViewResult Index()
		{
			return TabView(new { });
		}
	}
}