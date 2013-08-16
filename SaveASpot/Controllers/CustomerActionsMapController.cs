using System.Linq;
using System.Web.Mvc;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[CustomerAuthorize]
	public sealed class CustomerActionsMapController : BaseController
	{
		private readonly ICustomerActionsMapControllerService _customerActionsMapControllerService;

		public CustomerActionsMapController(ICustomerActionsMapControllerService customerActionsMapControllerService)
		{
			_customerActionsMapControllerService = customerActionsMapControllerService;
		}

		[HttpPost]
		public JsonResult BookingSpots(IElementIdentity[] identities)
		{
			var bookedSpotsViewModel = _customerActionsMapControllerService.BookingSpots(identities);

			return Json(new { identities = bookedSpotsViewModel.BookedSpots.Select(e => e.ToString()).ToArray(), cart = bookedSpotsViewModel.Cart.AsCartJson() });
		}

		[HttpPost]
		public JsonResult RemoveBookedSpot(IElementIdentity bookedSpotIdentity)
		{
			var bookedSpotsViewModel = _customerActionsMapControllerService.RemoveBookedSpot(bookedSpotIdentity);

			return Json(new { identities = bookedSpotsViewModel.BookedSpots.Select(e => e.ToString()).ToArray(), cart = bookedSpotsViewModel.Cart.AsCartJson() });
		}
	}
}