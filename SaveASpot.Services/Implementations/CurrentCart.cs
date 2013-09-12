using System.Linq;
using SaveASpot.Core.Cart;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web;
using SaveASpot.Repositories.Interfaces.Carts;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class CurrentCart : ICurrentCart
	{
		private readonly ICartQueryable _cartQueryable;
		private readonly ICurrentSessionIdentity _currentSessionIdentity;
		private readonly ICartService _cartService;
		private readonly ITypeConverter<Repositories.Models.Security.Cart, Cart> _typeConverter;

		public CurrentCart(ICartQueryable cartQueryable,
			ICurrentSessionIdentity currentSessionIdentity,
			ICartService cartService,
			ITypeConverter<Repositories.Models.Security.Cart, Cart> typeConverter)
		{
			_cartQueryable = cartQueryable;
			_currentSessionIdentity = currentSessionIdentity;
			_cartService = cartService;
			_typeConverter = typeConverter;
		}

		public Cart Cart
		{
			get
			{
				var existsCarts =
					_cartQueryable.Filter(e => e.FilterByIdentity(_currentSessionIdentity.SessionIdentity)).Find().ToList();

				if (existsCarts.Any())
				{
					return _typeConverter.Convert(existsCarts.First());
				}

				return _cartService.CreateCart(_currentSessionIdentity.SessionIdentity);
			}
		}
	}
}