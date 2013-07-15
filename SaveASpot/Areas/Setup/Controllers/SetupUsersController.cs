using System.Web.Mvc;
using SaveASpot.Areas.Setup.Controllers.Artifacts;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels;

namespace SaveASpot.Areas.Setup.Controllers
{
	public sealed class SetupUsersController : SetupController
	{
		private readonly ISetupUsersControllerService _setupUsersControllerService;

		public SetupUsersController(ISetupConfiguration setupConfiguration, ISetupUsersControllerService setupUsersControllerService)
			: base(setupConfiguration)
		{
			_setupUsersControllerService = setupUsersControllerService;
		}

		public ActionResult Index()
		{
			if (_setupUsersControllerService.AdminExists().IsSuccess)
			{
				return RedirectToAction("Index", "Home", new { area = string.Empty });
			}

			return View();
		}

		public ActionResult AddAdmin(RegisterViewModel registerViewModel)
		{
			if (ModelState.IsValid && !_setupUsersControllerService.AdminExists().IsSuccess)
			{
				var registrationResult = _setupUsersControllerService.RegisterAdmin(registerViewModel);
				if (registrationResult.IsSuccess)
				{
					return RedirectToAction("Index", "Home", new { area = string.Empty });
				}

				ModelState.AddModelError(string.Empty, registrationResult.Status.Message);
			}

			return View("Index", registerViewModel);
		}
	}
}