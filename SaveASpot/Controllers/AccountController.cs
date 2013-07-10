using System.Web.Mvc;
using System.Web.Security;
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
					return RedirectToAction("Index", "Home");
				}
				ModelState.AddModelError("", logOnResult.Status.Message);
			}

			// If we got this far, something failed, redisplay form
			return View(viewModel);
		}

		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();

			return RedirectToAction("Index", "Home");
		}

		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Register(RegisterViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var registerResult = _accountControllerService.RegisterUser(viewModel);

				if (registerResult.IsSuccess)
				{
					return RedirectToAction("Index", "Home");
				}

				ModelState.AddModelError("", registerResult.Status.Message);
			}

			// If we got this far, something failed, redisplay form
			return View(viewModel);
		}

		[Authorize]
		public ActionResult ChangePassword()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var changePasswordResult = _accountControllerService.ChangePassword(model);

				if (changePasswordResult.IsSuccess)
				{
					return RedirectToAction("ChangePasswordSuccess");
				}

				ModelState.AddModelError("", changePasswordResult.Status.Message);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		public ActionResult ChangePasswordSuccess()
		{
			return View();
		}
	}

	[AdministratorAuthorize]
	public sealed class TestController : BaseController
	{
		public ActionResult Index()
		{
			return Json(new { result = true }, JsonRequestBehavior.AllowGet);
		}
	}
}
