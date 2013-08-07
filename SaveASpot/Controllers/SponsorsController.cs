using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels;

namespace SaveASpot.Controllers
{
	[MainMenuTab(Alias = SiteConstants.SponsorsControllerAlias, Area = "", IndexOfOrder = 40, Title = "SponsorsTabTitle")]
	[AdministratorAuthorize]
	public sealed class SponsorsController : TabController
	{
		private readonly ISponsorsControllerService _sponsorsControllerService;

		public SponsorsController(ISponsorsControllerService sponsorsControllerService)
		{
			_sponsorsControllerService = sponsorsControllerService;
		}

		public ActionResult Index()
		{
			return TabView(_sponsorsControllerService.GetSponsors());
		}

		[HttpGet]
		public ViewResult CreateSponsor()
		{
			return View(new CreateSponsorViewModel());
		}

		[HttpPost]
		public ActionResult CreateSponsor(CreateSponsorViewModel createSponsorViewModel)
		{
			var createSponsorResult = _sponsorsControllerService.AddSponsor(createSponsorViewModel);

			return Json(new { status = createSponsorResult.IsSuccess, message = createSponsorResult.Status.Message });
		}
	}
}
