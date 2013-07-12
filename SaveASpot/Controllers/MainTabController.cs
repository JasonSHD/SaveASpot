using System.Web.Mvc;

namespace SaveASpot.Controllers
{
	public sealed class MainTabController : AdminTabController
	{
		public ViewResult Index()
		{
			return TabView(new { });
		}
	}
}