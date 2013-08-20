﻿using System.Linq;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core;
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

		[HttpPost]
		public ViewResult Remove(IElementIdentity identity, SelectorViewModel selectorViewModel)
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