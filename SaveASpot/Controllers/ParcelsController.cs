using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;
<<<<<<< HEAD
using SaveASpot.ViewModels.PhasesAndParcels;
=======
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937

namespace SaveASpot.Controllers
{
	[AdministratorAuthorize]
<<<<<<< HEAD
	[PhasePageTab(Alias = SiteConstants.ParcelsControllerAlias, IndexOfOrder = 20, Title = "ParcelsAccordionGroupTitle")]
=======
	[PhasePageTab(Alias = "parcelsAccordionGroup", IndexOfOrder = 20, Title = "ParcelsAccordionGroupTitle")]
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937
	public sealed class ParcelsController : TabController
	{
		private readonly IParcelsControllerService _parcelsControllerService;

		public ParcelsController(IParcelsControllerService parcelsControllerService)
		{
			_parcelsControllerService = parcelsControllerService;
		}

<<<<<<< HEAD
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
		public ViewResult Remove(string identity, SelectorViewModel selectorViewModel)
		{
			_parcelsControllerService.Remove(identity);

			if (!ModelState.IsValid)
			{
				selectorViewModel.Erase();
			}

			var model = _parcelsControllerService.GetParcels(selectorViewModel);

			return TabView("Index", model);
=======
		public ViewResult Index()
		{
			return TabView(_parcelsControllerService.GetParcels());
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937
		}
	}
}