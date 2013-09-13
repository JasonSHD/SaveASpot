using System.Linq;
using System.Web.Mvc;
using SaveASpot.Core;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[AdministratorAuthorize]
	public sealed class AdministratorActionsMapController : BaseController
	{
		private readonly IAdministratorActionsMapControllerService _administratorActionsMapControllerService;

		public AdministratorActionsMapController(IAdministratorActionsMapControllerService administratorActionsMapControllerService)
		{
			_administratorActionsMapControllerService = administratorActionsMapControllerService;
		}

		[HttpPost]
		public JsonResult BookingSpots(IElementIdentity[] identities, IElementIdentity sponsorIdentity)
		{
			var bookedSpotsViewModel = _administratorActionsMapControllerService.BookingSpots(identities, sponsorIdentity);

			return Json(new { bookedSpots = bookedSpotsViewModel.BookedSpots.Select(e => e.ToString()) });
		}
	}
}