using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;
using Stripe;

namespace SaveASpot.Controllers
{
	[CustomerAuthorize]
	[AdministratorAuthorize]
	public class StripeController : Controller
	{
		private readonly ICustomerService _customerService;

		public StripeController(ICustomerService customerService)
		{
			_customerService = customerService;
		}

		
		public JsonResult IsPaymentInformationAdded()
		{
			var customer = _customerService.GetCustomerById(ControllerContext.HttpContext.User.Identity.Name);

			var result = !string.IsNullOrEmpty(customer.StripeUserId);
			
			return Json(result, JsonRequestBehavior.AllowGet);

		}

		[HttpGet]
		public ViewResult CreatePaymentInformation()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreatePaymentInformation(string token)
		{
			//var customer = new StripeCustomerCreateOptions { TokenId = token, Description = "test mastercard user", Email = "test@mail.com" };

			//var stripeCustomerService = new StripeCustomerService();
			//var stripeCustomer = stripeCustomerService.Create(customer);

			var result = _customerService.UpdateSiteCustomer(ControllerContext.HttpContext.User.Identity.Name, token);


			return Json(new {result });
		}
	}
}
