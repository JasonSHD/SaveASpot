using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return View();
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Test()
		{
			return View();
		}
	}
}
