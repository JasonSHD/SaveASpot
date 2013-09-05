using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.Carts;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class CartService : ICartService
	{
		private readonly ICartRepository _cartRepository;
		private readonly ITypeConverter<Repositories.Models.Security.Cart, Cart> _typeConverter;

		public CartService(ICartRepository cartRepository, ITypeConverter<Repositories.Models.Security.Cart, Cart> typeConverter)
		{
			_cartRepository = cartRepository;
			_typeConverter = typeConverter;
		}

		public void AddSpotToCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity)
		{
			_cartRepository.AddSpotToCart(cartIdentity, spotIdentity);
		}

		public void RemoveSpotFromCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity)
		{
			_cartRepository.RemoveSpotFromCart(cartIdentity, spotIdentity);
		}

		public Cart CreateCart(IElementIdentity elementIdentity)
		{
			var cart = _cartRepository.CreateCart(elementIdentity);
			return _typeConverter.Convert(cart);
		}
	}
}