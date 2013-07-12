using System.Web.Mvc;

namespace SaveASpot.Controllers
{
	public sealed class PhasesController : AdminTabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}
	}
}