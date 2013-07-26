using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[AdministratorAuthorize]
	[PhasePageTab(Alias = SiteConstants.UploadPhasesAndParcelsControllerAlias, IndexOfOrder = 40, Title = "UploadPhasesAndParcelsAccordionGroupTitle")]
	public sealed class UploadPhasesAndParcelsController : TabController
	{
		private readonly IUploadPhasesAndParcelsControllerService _uploadPhasesAndParcelsControllerService;

		public UploadPhasesAndParcelsController(IUploadPhasesAndParcelsControllerService uploadPhasesAndParcelsControllerService)
		{
			_uploadPhasesAndParcelsControllerService = uploadPhasesAndParcelsControllerService;
		}

		public ViewResult Index()
		{
			return TabView(new { });
		}

		[HttpPost]
		public ActionResult UploadSpots(IEnumerable<HttpPostedFileBase> postedSpots)
		{
			var result = _uploadPhasesAndParcelsControllerService.AddSpots(postedSpots);

			return Json(new { status = result.IsSuccess, files = result.Status.ToArray() });
		}

		[HttpPost]
		public JsonResult UploadParcels(IEnumerable<HttpPostedFileBase> postedParcels)
		{
			var result = _uploadPhasesAndParcelsControllerService.AddParcels(postedParcels);

			return Json(new { status = result.IsSuccess, files = result.Status.ToArray() });
		}
	}
}