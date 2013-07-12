using System.Web.Mvc;

namespace SaveASpot.Controllers
{
	public sealed class MapController : AdminTabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}
	}
}