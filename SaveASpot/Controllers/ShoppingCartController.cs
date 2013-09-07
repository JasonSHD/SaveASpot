using System.Web.Mvc;

namespace SaveASpot.Controllers
{
	public class ShoppingCartController : Controller
	{
		//
		// GET: /ShoppingCart/

		public ActionResult Index()
		{
			return View();
		}
		public ActionResult Success()
		{
			return View();
		}
	}
}
