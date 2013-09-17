using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers.Checkout;
using SaveASpot.ViewModels.Checkout;
using Stripe;

namespace SaveASpot.Services.Implementations.Controllers.Checkout
{
	public sealed class CheckoutControllerService : ICheckoutControllerService
	{
		private readonly ICurrentCart _currentCart;
		private readonly ICurrentCustomer _currentCustomer;
		private readonly ICartService _cartService;
		private readonly IElementIdentityConverter _elementIdentityConverter;
		private readonly ISpotRepository _spotRepository;
		private readonly ISpotPhaseContainerQueryable _spotPhaseContainerQueryable;
		private readonly IParcelQueryable _parcelQueryable;
		private readonly ISpotQueryable _spotQueryable;
		private readonly ICustomerQueryable _customerQueryable;

		public CheckoutControllerService(ICurrentCart currentCart,
			ICurrentCustomer currentCustomer,
			ICartService cartService,
			IElementIdentityConverter elementIdentityConverter,
			ISpotRepository spotRepository,
			ISpotPhaseContainerQueryable spotPhaseContainerQueryable,
			IParcelQueryable parcelQueryable,
			ISpotQueryable spotQueryable,
			ICustomerQueryable customerQueryable)
		{
			_currentCart = currentCart;
			_currentCustomer = currentCustomer;
			_cartService = cartService;
			_elementIdentityConverter = elementIdentityConverter;
			_spotRepository = spotRepository;
			_spotPhaseContainerQueryable = spotPhaseContainerQueryable;
			_parcelQueryable = parcelQueryable;
			_spotQueryable = spotQueryable;
			_customerQueryable = customerQueryable;
		}

		public CheckoutResultViewModel Checkout(IElementIdentity[] spotIdentitiesForCheckout)
		{
			var cartIdentity = _currentCart.Cart.Identity;
			var currentCustomerIdentity = _currentCustomer.Customer.Identity;
			var spotPhaseContainersForCheckout = _spotPhaseContainerQueryable.Find(spotIdentitiesForCheckout);


			foreach (var container in spotPhaseContainersForCheckout)
			{
				_cartService.RemoveSpotFromCart(cartIdentity, _elementIdentityConverter.ToIdentity(container.Spot.Id));
				_spotRepository.MapSpotToCustomer(container.Spot, currentCustomerIdentity, container.Phase.SpotPrice.HasValue ? container.Phase.SpotPrice : null);
			}

			return new CheckoutResultViewModel { IsSuccess = true, Cart = _currentCart.Cart };
		}

		public IMethodResult CheckOutPhase(IElementIdentity phaseId)
		{
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
				var spotPriceInCents = spot.SpotPrice * 100;
				var totalAmountInCents = (countOfSpots * spotPriceInCents).ToString();
				var spot1 = spot;
				var customer = _customerQueryable.Filter(e => e.FilterById(_elementIdentityConverter.ToIdentity(spot1.CustomerId))).First();

				if (!string.IsNullOrEmpty(customer.StripeUserId))
				{
					var myCharge = new StripeChargeCreateOptions { AmountInCents = int.Parse(totalAmountInCents), Currency = "usd", CustomerId = customer.StripeUserId };

					var chargeService = new StripeChargeService();

					chargeService.Create(myCharge);
				}
			}
			return new MessageMethodResult(true, string.Empty);
		}
	}
}