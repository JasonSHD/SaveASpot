using SaveASpot.Core;
using SaveASpot.ViewModels.Checkout;

namespace SaveASpot.Services.Interfaces.Controllers.Checkout
{
	public interface ICheckoutController
	{
		CheckoutResultViewModel Checkout(IElementIdentity[] spotsForCheckout);
	}
}