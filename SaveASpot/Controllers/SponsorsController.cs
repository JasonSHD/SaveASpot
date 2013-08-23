using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core;
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

		[HttpGet]
		[CustomerAuthorize]
		public JsonResult SponsorDetails(IElementIdentity sponsorIdentity)
		{
			return Json(_sponsorsControllerService.SponsorDetails(sponsorIdentity).AsSponsorJson(), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ViewResult EditSponsor(SponsorViewModel sponsorViewModel)
		{
			return View(sponsorViewModel);
		}

		[HttpPost]
		public ActionResult EditSponsor(string identity, SponsorViewModel sponsorViewModel)
		{
			var updateSponsorResult = _sponsorsControllerService.EditSponsor(identity, sponsorViewModel);

			return Json(new { status = updateSponsorResult.IsSuccess, message = updateSponsorResult.Status.Message });
		}

		[HttpPost]
		public ViewResult Remove(string identity)
		{
			_sponsorsControllerService.Remove(identity);

			var model = _sponsorsControllerService.GetSponsors();

			return TabView("Index", model);
		}
    }
}
