using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Test()
		{
			return View();
		}
	}
}
