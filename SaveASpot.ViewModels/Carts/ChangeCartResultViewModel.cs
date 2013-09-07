using SaveASpot.Core.Security;

namespace SaveASpot.ViewModels.Carts
{
	public sealed class ChangeCartResultViewModel
	{
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
		public Cart Cart { get; set; }

		public object AsJson()
		{
			return new { isSuccess = IsSuccess, message = Message, cart = Cart.AsCartJson() };
		}
	}
}
