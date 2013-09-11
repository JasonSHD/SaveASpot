using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Controllers.Checkout;
using SaveASpot.ViewModels.Checkout;

namespace SaveASpot.Services.Implementations.Controllers.Checkout
{
	public sealed class CheckoutController : ICheckoutController
	{
		public CheckoutResultViewModel Checkout(IElementIdentity[] spotsForCheckout)
		{
			return new CheckoutResultViewModel { IsSuccess = true };
		}
	}
}