using System.Linq;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core;
using SaveASpot.Core.Geocoding;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Controllers
{
	[MainMenuTab(Alias = SiteConstants.MapControllerAlias, Area = "", IndexOfOrder = 10, Title = "MapTabTitle")]
	[CustomerAuthorize]
	[AdministratorAuthorize]
	[AnonymAuthorize]
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

		public JsonResult ForSquare(IElementIdentity phaseIdentity, Point topRight, Point bottomLeft)
		{
			var result = _mapControllerService.ForSquare(phaseIdentity, topRight, bottomLeft);

			return Json(new
			{
				status = result.Status.ToString(),
				center = result.Center.ToJson(),
				message = result.Message,
				spots = result.Spots.Select(e => e.ToJson()).ToList(),
				parcels = result.Parcels.Select(e => e.ToJson()).ToList(),
			}, JsonRequestBehavior.AllowGet);
		}
	}
}