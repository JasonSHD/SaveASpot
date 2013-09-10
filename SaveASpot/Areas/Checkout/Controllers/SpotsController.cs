using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers.Checkout;

namespace SaveASpot.Areas.Checkout.Controllers
{
	[CustomerAuthorize]
	public sealed class SpotsController : BaseController
	{
		private readonly ISpotsControllerService _spotsControllerService;

		public SpotsController(ISpotsControllerService spotsControllerService)
		{
			_spotsControllerService = spotsControllerService;
		}

		[AnonymAuthorize]
		[HttpGet]
		public ViewResult SpotsInCart()
		{
			return View(_spotsControllerService.GetSpots());
		}
	}
}