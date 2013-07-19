using System.Linq;
using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers.Artifacts
{
	public abstract class AdminTabController : TabController
	{
		protected override ViewResult TabView(object model)
		{
			var specifiedMasterName = Request.Headers.AllKeys.All(e => e != SiteConstants.AdminTabControlHeader);

			var view = View(null, specifiedMasterName ? SiteConstants.AdminTabMasterName : null, model);
			return view;
		}
	}
}