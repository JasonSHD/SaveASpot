using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core;
using SaveASpot.Core.Geocoding;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Controllers
{
	[AdministratorAuthorize]
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

		[CustomerAuthorize]
		[AnonymAuthorize]
		public ContentResult ByPhase(IElementIdentity identity)
		{
			return new ContentResult
			{
				Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(_spotsControllerService.ByPhase(identity).Spots.Select(e => e.ToJson())),
				ContentType = "application/json"
			};
		}

		[HttpPost]
		public ViewResult Remove(IElementIdentity identity, SelectorViewModel selectorViewModel)
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