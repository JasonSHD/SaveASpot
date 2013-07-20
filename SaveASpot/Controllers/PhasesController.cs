<<<<<<< HEAD
﻿using System.Linq;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
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

		public ViewResult Index(SelectorViewModel selectorViewModel)
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

			return TabView(model);
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
	}
=======
﻿using System.Web.Mvc;
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
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937
}