using System.Web.Mvc;
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

		public JsonResult BookingSpots(string[] identities)
		{
			_customerActionsMapControllerService.BookingSpots(identities);

			return Json(new { });
		}
	}
}