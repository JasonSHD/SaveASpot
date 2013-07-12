using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	public abstract class AdminTabController : BaseController
	{
		private readonly IEnumerable<TabDescription> _tabDescriptions;

		protected AdminTabController()
		{
			_tabDescriptions = new[]
			{
				new TabDescription{ControllerType = typeof(MapController), Alias = SiteConstants.MapControllerAlias, Roles = new Role[0],Title = "Map"},
				new TabDescription{ControllerType = typeof(CustomersController), Alias = SiteConstants.CustomersControllerAlias, Roles = new Role[0], Title = "Customers"},
				new TabDescription{ControllerType = typeof(PhasesController), Alias = SiteConstants.PhasesControllerAlias, Roles = new Role[0], Title = "Phases & Parcels"},
			};
		}

		protected virtual ViewResult TabView(object model)
		{
			var specifiedMasterName = Request.Headers.AllKeys.All(e => e != SiteConstants.AdminTabControlHeader);
			TabDescription.SetDescriptions(_tabDescriptions, ViewBag);

			var view = View(null, specifiedMasterName ? SiteConstants.AdminTabMasterName : null, model);
			return view;
		}
	}
}