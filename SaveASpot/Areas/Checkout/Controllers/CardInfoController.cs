using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Areas.Checkout.Controllers
{
	[CustomerAuthorize]
	public sealed class CardInfoController : BaseController
	{
		[AnonymAuthorize]
		[HttpGet]
		public ViewResult CartInfo()
		{
			return View();
		}
	}
}