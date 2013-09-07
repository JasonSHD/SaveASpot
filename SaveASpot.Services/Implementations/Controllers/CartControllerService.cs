using SaveASpot.Core;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.Carts;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class CartControllerService : ICartControllerService
	{
		private readonly ICurrentCart _currentCart;
		private readonly ICartService _cartService;
		private readonly ITextService _textService;

		public CartControllerService(ICurrentCart currentCart,
			ICartService cartService,
			ITextService textService)
		{
			_currentCart = currentCart;
			_cartService = cartService;
			_textService = textService;
		}

		public AddSpotToCartResultViewModel AddSpotToCart(IElementIdentity spotIdentity)
		{
			var addResult = _cartService.AddSpotToCart(_currentCart.Cart.Identity, spotIdentity);

			return new AddSpotToCartResultViewModel { Cart = _currentCart.Cart, IsSuccess = addResult.IsSuccess, Message = _textService.ResolveTest(addResult.Status) };
		}

		public void RemoveSpotFromCart(IElementIdentity spotIdentity)
		{
			_cartService.RemoveSpotFromCart(_currentCart.Cart.Identity, spotIdentity);
		}
	}
}