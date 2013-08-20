using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.ViewModels;
using Stripe;

namespace SaveASpot.Controllers
{
	[CustomerAuthorize]
	[AdministratorAuthorize]
	public class StripeCheckOutController : Controller
	{
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

			var r = ControllerContext.HttpContext.User.Identity;

			return new JsonResult();
		}

	}
}
