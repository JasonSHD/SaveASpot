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
				// Attempt to register the user
				//MembershipCreateStatus createStatus;
				//Membership.CreateUser(viewModel.UserName, viewModel.Password, viewModel.Email, null, null, true, null, out createStatus);

				//if (createStatus == MembershipCreateStatus.Success)
				//{
				//	FormsAuthentication.SetAuthCookie(viewModel.UserName, false /* createPersistentCookie */);
				//	return RedirectToAction("Index", "Home");
				//}
				//else
				//{
				//	ModelState.AddModelError("", ErrorCodeToString(createStatus));
				//}

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

		#region Status Codes
		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "User name already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}
		#endregion
	}
}
