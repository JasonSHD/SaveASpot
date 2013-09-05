using System.Collections.Generic;

namespace SaveASpot.Core.Security
{
	public sealed class Cart
	{
		private readonly IElementIdentity _cartIdentity;
		private readonly IEnumerable<IElementIdentity> _elementIdentities;
		public IEnumerable<IElementIdentity> ElementIdentities { get { return _elementIdentities; } }
		public IElementIdentity Identity { get { return _cartIdentity; } }

		public Cart(IElementIdentity cartIdentity, IEnumerable<IElementIdentity> elementIdentities)
		{
			_cartIdentity = cartIdentity;
			_elementIdentities = elementIdentities;
		}
	}
}