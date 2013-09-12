using System.Collections.Generic;

namespace SaveASpot.Core.Cart
{
	public sealed class Cart
	{
		private readonly IElementIdentity _cartIdentity;
		private readonly IEnumerable<SpotElement> _spotElements;
		private readonly decimal _cartPrice;
		public IElementIdentity Identity { get { return _cartIdentity; } }
		public IEnumerable<SpotElement> SpotElements { get { return _spotElements; } }
		public decimal Price { get { return _cartPrice; } }

		public Cart(IElementIdentity cartIdentity, IEnumerable<SpotElement> spotElements, decimal cartPrice)
		{
			_cartIdentity = cartIdentity;
			_spotElements = spotElements;
			_cartPrice = cartPrice;
		}
	}
}