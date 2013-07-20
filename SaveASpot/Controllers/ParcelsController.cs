using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

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

		public ViewResult Index()
		{
			return TabView(_parcelsControllerService.GetParcels());
		}
	}
}