using System.Web.Mvc;
using SaveASpot.Core;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	public class StripeController : Controller
	{
		private readonly IStripeControllerService _stripeControllerService;

		public StripeController(IStripeControllerService stripeControllerService)
		{
			_stripeControllerService = stripeControllerService;
		}

		[CustomerAuthorize]
		[AdministratorAuthorize]
		public JsonResult IsPaymentInformationAdded()
		{
			var result = _stripeControllerService.IsPaymentInformationAdded(ControllerContext.HttpContext.User.Identity.Name);
			return Json(result.IsSuccess, JsonRequestBehavior.AllowGet);
		}

		[CustomerAuthorize]
		[AdministratorAuthorize]
		[HttpPost]
		public ActionResult CreatePaymentInformation(string token)
		{
			var result = _stripeControllerService.CreatePaymentInformation(token,
																																		 ControllerContext.HttpContext.User.Identity.Name);
			return Json(new { result });
		}


		[AdministratorAuthorize]
		public ActionResult CheckOutPhase(IElementIdentity phaseId, string spotPrice)
		{
			var result = _stripeControllerService.CheckOutPhase(phaseId, spotPrice);
			return Json(new {result});
		}
	}
}
