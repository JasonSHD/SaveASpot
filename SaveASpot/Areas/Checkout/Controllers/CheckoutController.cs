using System.Web.Mvc;
using SaveASpot.Core;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers.Checkout;

namespace SaveASpot.Areas.Checkout.Controllers
{
	[CustomerAuthorize]
	public sealed class CheckoutController : BaseController
	{
		private readonly ICheckoutController _checkoutController;

		public CheckoutController(ICheckoutController checkoutController)
		{
			_checkoutController = checkoutController;
		}

		[HttpPost]
		public JsonResult Index(IElementIdentity[] spotsForCheckout)
		{
			return Json(_checkoutController.Checkout(spotsForCheckout).AsJson());
		}
	}
}
