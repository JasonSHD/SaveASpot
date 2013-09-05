using System.Web.Mvc;
using SaveASpot.Core;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[CustomerAuthorize]
	[AdministratorAuthorize]
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
			_cartControllerService.AddSpotToCart(spotIdentity);

			return Json(new object());
		}

		[HttpPost]
		public JsonResult RemoveSpotFromCart(IElementIdentity spotIdentity)
		{
			_cartControllerService.RemoveSpotFromCart(spotIdentity);

			return Json(new object());
		}
	}
}