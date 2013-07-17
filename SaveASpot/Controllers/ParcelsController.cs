using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[AdministratorAuthorize]
	public sealed class ParcelsController : BaseController
	{
		private readonly IParcelsControllerService _parcelsControllerService;

		public ParcelsController(IParcelsControllerService parcelsControllerService)
		{
			_parcelsControllerService = parcelsControllerService;
		}

		public ViewResult Index()
		{
			return View();
		}
	}
}