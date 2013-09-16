using System.Web.Mvc;
using SaveASpot.Core.Configuration;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Areas.Checkout.Controllers
{
	[CustomerAuthorize]
	public sealed class CardInfoController : BaseController
	{
		private readonly IConfigurationManager _configurationManager;
		private readonly ICardInfoControllerService _cardInfoControllerService;

		public CardInfoController(IConfigurationManager configurationManager, ICardInfoControllerService cardInfoControllerService)
		{
			_configurationManager = configurationManager;
			_cardInfoControllerService = cardInfoControllerService;
		}

		[AnonymAuthorize]
		[HttpGet]
		public ViewResult CreateCardInfo()
		{
			return View();
		}

		[AnonymAuthorize]
		[HttpGet]
		public ActionResult GetStripePublicKey()
		{
			var key = _configurationManager.GetSettings("StripePublicKey");

			return Json(new { key }, JsonRequestBehavior.AllowGet);
		}

		[AnonymAuthorize]
		[HttpPost]
		public ActionResult CreatePaymentInformation(string token)
		{
			var result = _cardInfoControllerService.CreatePaymentInformation(token);
			return Json(new { result });
		}
	}
}