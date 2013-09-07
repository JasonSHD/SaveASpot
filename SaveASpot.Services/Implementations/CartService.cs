using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Interfaces.Carts;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class CartService : ICartService
	{
		private readonly ICartRepository _cartRepository;
		private readonly ITypeConverter<Repositories.Models.Security.Cart, Cart> _typeConverter;
		private readonly ISpotValidateFactory _spotValidateFactory;
		private readonly ISpotQueryable _spotQueryable;

		public CartService(ICartRepository cartRepository,
			ITypeConverter<Repositories.Models.Security.Cart, Cart> typeConverter,
			ISpotValidateFactory spotValidateFactory,
			ISpotQueryable spotQueryable)
		{
			_cartRepository = cartRepository;
			_typeConverter = typeConverter;
			_spotValidateFactory = spotValidateFactory;
			_spotQueryable = spotQueryable;
		}

		public IMethodResult<string> AddSpotToCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity)
		{
			var spot = _spotQueryable.Filter(e => e.ByIdentity(spotIdentity)).First();
			if (_spotValidateFactory.Available().Validate(new SpotArg { Spot = spot }).IsValid)
			{
				_cartRepository.AddSpotToCart(cartIdentity, spotIdentity);

				return new MethodResult<string>(true, "AddSpotToCartSuccess");
			}

			return new MethodResult<string>(false, "SpotNotAvailableToAddToCart");
		}

		public IMethodResult<string> RemoveSpotFromCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity)
		{
			_cartRepository.RemoveSpotFromCart(cartIdentity, spotIdentity);

			return new MethodResult<string>(true, string.Empty);
		}

		public Cart CreateCart(IElementIdentity elementIdentity)
		{
			var cart = _cartRepository.CreateCart(elementIdentity);
			return _typeConverter.Convert(cart);
		}
	}
}