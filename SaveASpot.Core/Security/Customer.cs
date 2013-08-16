using System.Collections.Generic;

namespace SaveASpot.Core.Security
{
	public sealed class Customer
	{
		private readonly IElementIdentity _identity;
		private readonly User _user;
		private readonly Cart _cart;
		public User User { get { return _user; } }
		public IElementIdentity Identity { get { return _identity; } }
		public Cart Cart { get { return _cart; } }

		public Customer(IElementIdentity identity, User user, Cart cart)
		{
			_identity = identity;
			_user = user;
			_cart = cart;
		}
	}

	public sealed class Cart
	{
		private readonly IEnumerable<IElementIdentity> _elementIdentities;
		public IEnumerable<IElementIdentity> ElementIdentities { get { return _elementIdentities; } }

		public Cart(IEnumerable<IElementIdentity> elementIdentities)
		{
			_elementIdentities = elementIdentities;
		}
	}
}