using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[TabDescriptions(SiteConstants.CustomersControllerAlias, "Customers")]
	[AdministratorAuthorize]
	public sealed class CustomersController : AdminTabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}
	}
}