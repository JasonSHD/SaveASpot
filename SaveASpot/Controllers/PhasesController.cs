using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Logging;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Controllers
{
	[PhasePageTab(Alias = "phasesAccordionGroup", IndexOfOrder = 20, Title = "PhasesAccordionGroupTitle")]
	[AdministratorAuthorize]
	public sealed class PhasesController : TabController
	{
		

		public ActionResult Index()
		{
			return TabView(new { });
		}

		public PhasesController(IArcgisService arcgisService, ILogger logger)
		{
			
		}

		
	}
}