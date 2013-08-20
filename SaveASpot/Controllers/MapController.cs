using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[MainMenuTab(Alias = SiteConstants.MapControllerAlias, Area = "", IndexOfOrder = 10, Title = "MapTabTitle")]
	[CustomerAuthorize]
	[AdministratorAuthorize]
	public sealed class MapController : TabController
	{
		private readonly IMapControllerService _mapControllerService;

		public MapController(IMapControllerService mapControllerService)
		{
			_mapControllerService = mapControllerService;
		}

		public ActionResult Index()
		{
			return TabView(_mapControllerService.GetMapViewModel());
		}
	}
}