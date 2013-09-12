using SaveASpot.Core.Cart;
using SaveASpot.Core.Security;

namespace SaveASpot.ViewModels.ViewExtensions
{
	public sealed class UserCartViewModel
	{
		public User User { get; set; }
		public Cart Cart { get; set; }
	}
}