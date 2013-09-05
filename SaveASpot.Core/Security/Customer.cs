namespace SaveASpot.Core.Security
{
	public sealed class Customer
	{
		private readonly IElementIdentity _identity;
		private readonly User _user;
		public User User { get { return _user; } }
		public IElementIdentity Identity { get { return _identity; } }

		public Customer(IElementIdentity identity, User user)
		{
			_identity = identity;
			_user = user;
		}
	}
}