using SaveASpot.Core;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class CartControllerService : ICartControllerService
	{
		private readonly ICurrentCart _currentCart;
		private readonly ICartService _cartService;

		public CartControllerService(ICurrentCart currentCart, ICartService cartService)
		{
			_currentCart = currentCart;
			_cartService = cartService;
		}

		public void AddSpotToCart(IElementIdentity spotIdentity)
		{
			_cartService.AddSpotToCart(_currentCart.Cart.Identity, spotIdentity);
		}

		public void RemoveSpotFromCart(IElementIdentity spotIdentity)
		{
			_cartService.RemoveSpotFromCart(_currentCart.Cart.Identity, spotIdentity);
		}
	}
}