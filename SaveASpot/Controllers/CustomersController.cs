using System.Web.Mvc;

namespace SaveASpot.Controllers
{
	public sealed class CustomersController : AdminTabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}
	}
}