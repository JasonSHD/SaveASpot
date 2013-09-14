namespace SaveASpot.Core.Security
{
	public sealed class Customer
	{
		private readonly IElementIdentity _identity;
		private readonly User _user;
		private readonly bool _isPaymentInfoAdded;
		public User User { get { return _user; } }
		public IElementIdentity Identity { get { return _identity; } }
		public bool IsPaymentInfoAdded { get { return _isPaymentInfoAdded; } }

		public Customer(IElementIdentity identity, User user, bool isPaymentInfoAdded)
		{
			_identity = identity;
			_user = user;
			_isPaymentInfoAdded = isPaymentInfoAdded;
		}
	}
}