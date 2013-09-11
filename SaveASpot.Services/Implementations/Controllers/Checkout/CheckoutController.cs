using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers.Checkout;
using SaveASpot.ViewModels.Checkout;

namespace SaveASpot.Services.Implementations.Controllers.Checkout
{
	public sealed class CheckoutController : ICheckoutController
	{
		private readonly ICurrentCart _currentCart;
		private readonly ICurrentCustomer _currentCustomer;
		private readonly ISpotQueryable _spotQueryable;
		private readonly ICartService _cartService;
		private readonly IElementIdentityConverter _elementIdentityConverter;
		private readonly ISpotRepository _spotRepository;

		public CheckoutController(ICurrentCart currentCart, ICurrentCustomer currentCustomer, ISpotQueryable spotQueryable, ICartService cartService, IElementIdentityConverter elementIdentityConverter, ISpotRepository spotRepository)
		{
			_currentCart = currentCart;
			_currentCustomer = currentCustomer;
			_spotQueryable = spotQueryable;
			_cartService = cartService;
			_elementIdentityConverter = elementIdentityConverter;
			_spotRepository = spotRepository;
		}

		public CheckoutResultViewModel Checkout(IElementIdentity[] spotIdentitiesForCheckout)
		{
			var cartIdentity = _currentCart.Cart.Identity;
			var currentCustomerIdentity = _currentCustomer.Customer.Identity;
			var spotsForCheckout = _spotQueryable.Filter(e => e.ByIdentities(spotIdentitiesForCheckout));

			foreach (var spot in spotsForCheckout)
			{
				_cartService.RemoveSpotFromCart(cartIdentity, _elementIdentityConverter.ToIdentity(spot.Id));
				_spotRepository.MapSpotToCustomer(spot, currentCustomerIdentity);
			}

			return new CheckoutResultViewModel { IsSuccess = true, Cart = _currentCart.Cart };
		}
	}
}