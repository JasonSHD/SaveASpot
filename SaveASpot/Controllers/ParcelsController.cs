using System.Linq;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Controllers
{
	[AdministratorAuthorize]
	[PhasePageTab(Alias = SiteConstants.ParcelsControllerAlias, IndexOfOrder = 20, Title = "ParcelsAccordionGroupTitle")]
	public sealed class ParcelsController : TabController
	{
		private readonly IParcelsControllerService _parcelsControllerService;

		public ParcelsController(IParcelsControllerService parcelsControllerService)
		{
			_parcelsControllerService = parcelsControllerService;
		}

		public ViewResult Index(SelectorViewModel selectorViewModel)
		{
			if (!ModelState.IsValid)
			{
				selectorViewModel.Erase();
			}

			var model = _parcelsControllerService.GetParcels(selectorViewModel);

			return TabView(model);
		}

		public JsonResult ByPhase(string identity)
		{
			return Json(_parcelsControllerService.ByPhase(identity).Parcels.Select(e => e.ToJson()), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ViewResult Remove(string identity, SelectorViewModel selectorViewModel)
		{
			_parcelsControllerService.Remove(identity);

			if (!ModelState.IsValid)
			{
				selectorViewModel.Erase();
			}

			var model = _parcelsControllerService.GetParcels(selectorViewModel);

			return TabView("Index", model);
		}
	}
}