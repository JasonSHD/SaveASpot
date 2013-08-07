using System.Linq;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Controllers
{
	[AdministratorAuthorize]
	[CustomerAuthorize]
	[PhasePageTab(Alias = SiteConstants.SpotsControllerAlias, IndexOfOrder = 30, Title = "SpotsAccordingGroupTitle")]
	public sealed class SpotsController : TabController
	{
		private readonly ISpotsControllerService _spotsControllerService;

		public SpotsController(ISpotsControllerService spotsControllerService)
		{
			_spotsControllerService = spotsControllerService;
		}

		public ViewResult Index(SelectorViewModel selectorViewModel)
		{
			if (!ModelState.IsValid)
			{
				selectorViewModel.Erase();
			}

			var model = _spotsControllerService.GetSpots(selectorViewModel);

			return TabView(model);
		}

		public JsonResult ByPhase(string identity)
		{
			return Json(_spotsControllerService.ByPhase(identity).Spots.Select(e => e.ToJson()), JsonRequestBehavior.AllowGet);
		}

		public JsonResult BookingSpot(string identity)
		{
			var methodResult = _spotsControllerService.BookingSpot(identity);

			return Json(new { status = methodResult.IsSuccess, message = methodResult.Status });
		}

		[HttpPost]
		public ViewResult Remove(string identity, SelectorViewModel selectorViewModel)
		{
			_spotsControllerService.Remove(identity);

			if (!ModelState.IsValid)
			{
				selectorViewModel.Erase();
			}

			var model = _spotsControllerService.GetSpots(selectorViewModel);

			return TabView("Index", model);
		}
	}
}