using SaveASpot.Core.Security;
using SaveASpot.ViewModels.Account;

namespace SaveASpot.ViewModels.Checkout
{
	public sealed class UserInfoViewModel
	{
		public bool IsCustomer { get; set; }
		public User User { get; set; }
		public LogOnViewModel LogOnViewModel { get; set; }
	}
}