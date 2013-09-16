using SaveASpot.Core.Cart;
using SaveASpot.Core.Security;

namespace SaveASpot.ViewModels.ViewExtensions
{
	public sealed class CustomerCartViewModel
	{
		public Customer Customer { get; set; }
		public User Anonym { get; set; }
		public Cart Cart { get; set; }
	}
}