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

		public ChangeCartResultViewModel AddSpotToCart(IElementIdentity spotIdentity)
		{
			return Convert(_cartService.AddSpotToCart(_currentCart.Cart.Identity, spotIdentity));
		}

		public ChangeCartResultViewModel RemoveSpotFromCart(IElementIdentity spotIdentity)
		{
			return Convert(_cartService.RemoveSpotFromCart(_currentCart.Cart.Identity, spotIdentity));
		}

		private ChangeCartResultViewModel Convert(IMethodResult<string> result)
		{
			return new ChangeCartResultViewModel { Cart = _currentCart.Cart, IsSuccess = result.IsSuccess, Message = _textService.ResolveTest(result.Status) };
		}
	}
}