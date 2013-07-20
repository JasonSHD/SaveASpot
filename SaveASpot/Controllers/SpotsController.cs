using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[AdministratorAuthorize]
	[PhasePageTab(Alias = SiteConstants.SpotsControllerAlias, IndexOfOrder = 30, Title = "SpotsAccordingGroupTitle")]
	public sealed class SpotsController : TabController
	{
		public ViewResult Index()
		{
			return TabView(new { });
		}
	}
}