using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Security;
using Stripe;

namespace SaveASpot.Controllers
{

	public class StripeController : Controller
	{
		private readonly ICustomerService _customerService;
		private readonly IParcelService _parcelService;
		private readonly ISpotService _spotService;


		public StripeController(ICustomerService customerService, IParcelService parcelService, ISpotService spotService)
		{
			_customerService = customerService;
			_parcelService = parcelService;
			_spotService = spotService;
		}

		[CustomerAuthorize]
		[AdministratorAuthorize]
		public JsonResult IsPaymentInformationAdded()
		{
			var customer = _customerService.GetCustomerByUserId(ControllerContext.HttpContext.User.Identity.Name);

			var result = !string.IsNullOrEmpty(customer.StripeUserId);

			return Json(result, JsonRequestBehavior.AllowGet);

		}

		[CustomerAuthorize]
		[AdministratorAuthorize]
		[HttpGet]
		public ViewResult CreatePaymentInformation()
		{
			return View();
		}

		[CustomerAuthorize]
		[AdministratorAuthorize]
		[HttpPost]
		public ActionResult CreatePaymentInformation(string token)
		{
			var customer = new StripeCustomerCreateOptions { TokenId = token };

			var stripeCustomerService = new StripeCustomerService();
			var stripeCustomer = stripeCustomerService.Create(customer);

			var result = _customerService.UpdateSiteCustomer(ControllerContext.HttpContext.User.Identity.Name, stripeCustomer.Id);

			return Json(new { result });
		}


		[AdministratorAuthorize]
		public ActionResult CheckOutPhase(string phaseId, string spotPrice)
		{
			var spotPriceInCents = (double.Parse(spotPrice, CultureInfo.InvariantCulture) * 100).ToString();

			var parcels = _parcelService.GetAllParcelsByPhaseId(phaseId);

			var spots = new List<Spot>();

			foreach (var parcel in parcels)
			{
				spots.AddRange(_spotService.GetSpotsByParcelId(parcel.Id.ToString()));
			}
			var filteredSpots = (from s in spots where s.CustomerId != MongoDB.Bson.ObjectId.Empty select s).ToList();

			//get quantity of unique customers
			var uniqueCustomers = filteredSpots.GroupBy(s => s.CustomerId).Select(grp => grp.First());

			foreach (var spot in uniqueCustomers)
			{
				var spotsForUser = (from f in filteredSpots where f.CustomerId == spot.CustomerId select f).ToList();
				var countOfSpots = spotsForUser.ToArray().Length;
				var totalAmountInCents = countOfSpots*int.Parse(spotPriceInCents);
				var customer = _customerService.GetCustomerById(spot.CustomerId.ToString());

				if (!string.IsNullOrEmpty(customer.StripeUserId))
				{
					var myCharge = new StripeChargeCreateOptions { AmountInCents = totalAmountInCents, Currency = "usd", CustomerId = customer.StripeUserId };

					var chargeService = new StripeChargeService();

					chargeService.Create(myCharge);

					var spotIdCollection = customer.Cart.SpotIdCollection.ToList();

					foreach (var sp in spotsForUser)
					{
						spotIdCollection.Remove(sp.Id);
					}

					_customerService.UpdateCustomerCart(customer.Id.ToString(), spotIdCollection.ToArray());
				}
			}

			var result = "test";
			return Json(new { result });
		}
	}
}
