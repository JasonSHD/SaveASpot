using System.Web.Mvc;
using SaveASpot.Core;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[CustomerAuthorize]
	[AnonymAuthorize]
	public sealed class CartController : BaseController
	{
		private readonly ICartControllerService _cartControllerService;

		public CartController(ICartControllerService cartControllerService)
		{
			_cartControllerService = cartControllerService;
		}

		[HttpPost]
		public JsonResult AddSpotToCart(IElementIdentity spotIdentity)
		{
			return Json(_cartControllerService.AddSpotToCart(spotIdentity).AsJson());
		}

		[HttpPost]
		public JsonResult RemoveSpotFromCart(IElementIdentity spotIdentity)
		{
			return Json(_cartControllerService.RemoveSpotFromCart(spotIdentity).AsJson());
		}
	}
}