namespace SaveASpot.Core.Security
{
	public sealed class Customer
	{
		private readonly string _customerId;
		private readonly User _user;
		public User User { get { return _user; } }
		public string Identity { get { return _customerId; } }

		public Customer(string customerId, User user)
		{
			_customerId = customerId;
			_user = user;
		}
	}
}