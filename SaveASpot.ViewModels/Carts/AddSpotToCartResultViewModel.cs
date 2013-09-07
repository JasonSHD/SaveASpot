using SaveASpot.Core.Security;

namespace SaveASpot.ViewModels.Carts
{
	public sealed class AddSpotToCartResultViewModel
	{
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
		public Cart Cart { get; set; }
	}
}
