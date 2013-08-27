using System.Linq;
using System.Web.Mvc;
using MongoDB.Bson;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Controllers
{
	[PhasePageTab(Alias = SiteConstants.PhasesControllerAlias, IndexOfOrder = 10, Title = "PhasesAccordionGroupTitle")]
	[MainMenuTab(Alias = SiteConstants.PhasesAndParcelsControllerAlias, Area = "", IndexOfOrder = 20, Title = "PhasesAndParcelsTabTitle")]
	[AdministratorAuthorize]
	public sealed class PhasesController : TabController
	{
		private readonly IPhasesControllerService _phasesControllerService;

		public PhasesController(IPhasesControllerService phasesControllerService)
		{
			_phasesControllerService = phasesControllerService;
		}

		[CustomerAuthorize]
		public ActionResult Index(SelectorViewModel selectorViewModel, bool isJson = false)
		{
			if (!ModelState.IsValid)
			{
				selectorViewModel.Erase();
			}

			var model = _phasesControllerService.GetPhases(selectorViewModel);

			if (Request.Headers.AllKeys.Any(e => e == SiteConstants.MainMenuTabSpecificReadyAlias))
			{
				return View(null, SiteConstants.Layouts.ParcelsAndSpotsAjaxLayout, model);
			}

			return isJson ? (ActionResult)Json(model.Phases.Select(e => e.ToJson()), JsonRequestBehavior.AllowGet) : TabView(model);
		}

		[HttpPost]
		public ViewResult Remove(string identity, SelectorViewModel selectorViewModel)
		{
			_phasesControllerService.RemovePhases(identity);

			if (!ModelState.IsValid)
			{
				selectorViewModel.Erase();
			}

			var model = _phasesControllerService.GetPhases(selectorViewModel);

			return TabView("Index", model);
		}

		[HttpGet]
		public ViewResult EditPhase(PhaseViewModel phaseViewModel)
		{
			return View(phaseViewModel);
		}

		[HttpPost]
		public ActionResult EditPhase(string identity, PhaseViewModel phaseViewModel)
		{
			var result = _phasesControllerService.EditPhase(identity, phaseViewModel);

			return Json(new { status = result.IsSuccess, message = result.Status.Message });
		}
	}
}