using System.Web.Mvc;
using SaveASpot.Core;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers.Checkout;

namespace SaveASpot.Areas.Checkout.Controllers
{
	public sealed class CheckoutController : BaseController
	{
		private readonly ICheckoutControllerService _checkoutController;

		public CheckoutController(ICheckoutControllerService checkoutController)
		{
			_checkoutController = checkoutController;
		}

		[HttpPost]
		[CustomerAuthorize]
		public JsonResult Index(IElementIdentity[] spotsForCheckout)
		{
			return Json(_checkoutController.Checkout(spotsForCheckout).AsJson());
		}

		[AdministratorAuthorize]
		[HttpPost]
		public ActionResult CheckOutPhase(IElementIdentity phaseId)
		{
			var result = _checkoutController.CheckOutPhase(phaseId);
			return Json(result.IsSuccess);
		}
	}
}
