using System.Web.Mvc;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels;
using SaveASpot.ViewModels.Account;

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
		public JsonResult LogOnAdmin(LogOnViewModel logOn)
		{
			return Json(_accountControllerService.LogOnAdmin(logOn).AsJsonResult());
		}

		[HttpPost]
		public JsonResult LogOnCustomer(LogOnViewModel logOn)
		{
			return Json(_accountControllerService.LogOnCustomer(logOn).AsJsonResult());
		}

		[HttpPost]
		public JsonResult RegistrateCustomer(CreateCustomerViewModel createCustomerViewModel)
		{
			return Json(_accountControllerService.RegistrateCustomer(createCustomerViewModel).AsJsonResult());
		}

		[HttpPost]
		public ActionResult LogOff()
		{
			var logOffResult = _accountControllerService.LogOff();

			if (logOffResult.IsSuccess)
			{
				return Json(new { user = logOffResult.Status.User.AsUserJson(), fullUser = new { user = logOffResult.Status.User.AsUserJson() } });
			}

			return Json(new { result = logOffResult.IsSuccess });
		}
	}
}
