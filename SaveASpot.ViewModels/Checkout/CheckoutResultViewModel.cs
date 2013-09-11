using SaveASpot.Core.Security;

namespace SaveASpot.ViewModels.Checkout
{
	public sealed class CheckoutResultViewModel
	{
		public bool IsSuccess { get; set; }
		public Cart Cart { get; set; }

		public object AsJson()
		{
			return new { isSuccess = IsSuccess, cart = Cart.AsCartJson() };
		}
	}
}