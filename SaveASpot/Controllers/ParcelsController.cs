using System;
using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[AdministratorAuthorize]
	[PhasePageTab]
	public sealed class ParcelsController : TabController
	{
		private readonly IParcelsControllerService _parcelsControllerService;

		public ParcelsController(IParcelsControllerService parcelsControllerService)
		{
			_parcelsControllerService = parcelsControllerService;
		}

		[PhasePageDefaultTabAction]
		public ViewResult Index()
		{
			return TabView(_parcelsControllerService.GetParcels());
		}
	}

	[AdministratorAuthorize]
	[PhasePageTab]
	public sealed class PhasesController : TabController { }

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class PhasePageTabAttribute : TabAttribute
	{
		public PhasePageTabAttribute()
			: base(typeof(PhasePageDefaultTabActionAttribute))
		{
		}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class PhasePageDefaultTabActionAttribute : DefaultTabActionAttribute { }
}