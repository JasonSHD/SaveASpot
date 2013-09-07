using System.Web.Mvc;
using SaveASpot.Core;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[CustomerAuthorize]
	[AnonymAuthorize]
	public sealed class CheckoutController : BaseController
	{
		private readonly ICheckoutControllerService _checkoutControllerService;

		public CheckoutController(ICheckoutControllerService checkoutControllerService)
		{
			_checkoutControllerService = checkoutControllerService;
		}

		[HttpGet]
		public ViewResult Index(IElementIdentity phaseIdentity)
		{
			return View(_checkoutControllerService.GetSpots(phaseIdentity));
		}
	}
}