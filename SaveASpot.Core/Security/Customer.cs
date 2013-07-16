namespace SaveASpot.Core.Security
{
	public sealed class Customer
	{
		private readonly User _user;
		public User User { get { return _user; } }

		public Customer(User user)
		{
			_user = user;
		}
	}
}