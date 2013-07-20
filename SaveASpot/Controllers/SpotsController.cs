using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
<<<<<<< HEAD
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;
=======
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937

namespace SaveASpot.Controllers
{
	[AdministratorAuthorize]
<<<<<<< HEAD
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
=======
	[PhasePageTab(Alias = "spotsAccordionGroup", IndexOfOrder = 30, Title = "SpotsAccordingGroupTitle")]
	public sealed class SpotsController : TabController
	{
		public ViewResult Index()
		{
			return View();
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937
		}
	}
}