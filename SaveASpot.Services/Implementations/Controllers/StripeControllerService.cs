using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Security;
using Stripe;

namespace SaveASpot.Services.Implementations.Controllers
{
	public class StripeControllerService : IStripeControllerService
	{
		private readonly ICustomerService _customerService;
		private readonly IParcelQueryable _parcelQueryable;
		private readonly ISpotQueryable _spotQueryable;
		private readonly IElementIdentityConverter _elementIdentityConverter;
		private readonly ICurrentCart _currentCart;

		public StripeControllerService(ICustomerService customerService, IParcelQueryable parcelQueryable, ISpotQueryable spotQueryable, IElementIdentityConverter elementIdentityConverter, ICurrentCart currentCart)
		{
			_customerService = customerService;
			_parcelQueryable = parcelQueryable;
			_spotQueryable = spotQueryable;
			_elementIdentityConverter = elementIdentityConverter;
			_currentCart = currentCart;
		}

		public IMethodResult IsPaymentInformationAdded(string identityName)
		{
			var customer = _customerService.GetCustomerByUserId(identityName);

			return new MessageMethodResult(!string.IsNullOrEmpty(customer.StripeUserId), string.Empty);
		}

		public IMethodResult CreatePaymentInformation(string token, string identityName)
		{
			var customer = new StripeCustomerCreateOptions { TokenId = token };

			var stripeCustomerService = new StripeCustomerService();
			var stripeCustomer = stripeCustomerService.Create(customer);

			var result = _customerService.UpdateSiteCustomer(identityName, stripeCustomer.Id);
			return new MessageMethodResult(result, string.Empty);
		}

		public IMethodResult CheckOutPhase(IElementIdentity phaseId, string spotPrice)
		{
			var spotPriceInCents = (double.Parse(spotPrice, CultureInfo.InvariantCulture) * 100).ToString();
			var parcels = _parcelQueryable.Find(_parcelQueryable.ByPhase(phaseId)).Select(e => _elementIdentityConverter.ToIdentity(e.Id)).ToList();

			var spots = new List<Spot>();

			spots.AddRange(_spotQueryable.Filter(e => e.ByParcels(parcels)).Find());
			var filteredSpots = (from s in spots where s.CustomerId != MongoDB.Bson.ObjectId.Empty select s).ToList();

			//get quantity of unique customers
			var uniqueCustomers = filteredSpots.GroupBy(s => s.CustomerId).Select(grp => grp.First());

			foreach (var spot in uniqueCustomers)
			{
				var spotsForUser = (from f in filteredSpots where f.CustomerId == spot.CustomerId select f).ToList();
				var countOfSpots = spotsForUser.ToArray().Length;
				var totalAmountInCents = countOfSpots * int.Parse(spotPriceInCents);
				var customer = _customerService.GetCustomerById(spot.CustomerId.ToString());

				if (!string.IsNullOrEmpty(customer.StripeUserId))
				{
					var myCharge = new StripeChargeCreateOptions { AmountInCents = totalAmountInCents, Currency = "usd", CustomerId = customer.StripeUserId };

					var chargeService = new StripeChargeService();

					chargeService.Create(myCharge);

					throw new NotImplementedException("Please user CurrentCart.");
					//var spotIdCollection = customer.Cart.SpotIdCollection.ToList();

					//foreach (var sp in spotsForUser)
					//{
					//	spotIdCollection.Remove(sp.Id);
					//}

					//_customerService.UpdateCustomerCart(customer.Id.ToString(), spotIdCollection.ToArray());
				}
			}
			return new MessageMethodResult(true, string.Empty);
		}
	}
}
