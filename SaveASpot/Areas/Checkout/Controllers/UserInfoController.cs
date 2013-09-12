using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Areas.Checkout.Controllers
{
	public sealed class UserInfoController : BaseController
	{
		[HttpGet]
		public ActionResult UserInfo()
		{
			return View();
		}
	}
}