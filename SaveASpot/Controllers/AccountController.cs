using System.Web.Mvc;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels;

namespace SaveASpot.Controllers
{
	public class AccountController : BaseController
	{
		private readonly IAccountControllerService _accountControllerService;

		public AccountController(IAccountControllerService accountControllerService)
		{
			_accountControllerService = accountControllerService;
		}

		public ActionResult LogOn()
		{
			return View();
		}

		[HttpPost]
		public ActionResult LogOn(LogOnViewModel viewModel, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				var logOnResult = _accountControllerService.LogOn(viewModel);

				if (logOnResult.IsSuccess)
				{
					return Json(logOnResult.Status.User.AsUserJson());
				}

				return Json(new { status = false, message = logOnResult.Status.Message });
			}

			// If we got this far, something failed, redisplay form
			return View(viewModel);
		}

		[HttpPost]
		public ActionResult LogOff()
		{
			var logOffResult = _accountControllerService.LogOff();

			if (logOffResult.IsSuccess)
			{
				return Json(logOffResult.Status.User.AsUserJson());
			}

			return Json(new { result = logOffResult.IsSuccess });
		}
	}
}
