namespace SaveASpot.ViewModels.Checkout
{
	public sealed class CheckoutResultViewModel
	{
		public bool IsSuccess { get; set; }

		public object AsJson()
		{
			return new { isSuccess = IsSuccess };
		}
	}
}