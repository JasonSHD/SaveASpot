using SaveASpot.Core;
using SaveASpot.ViewModels.Checkout;

namespace SaveASpot.Services.Interfaces.Controllers.Checkout
{
	public interface ICheckoutControllerService
	{
		CheckoutResultViewModel Checkout(IElementIdentity[] spotsForCheckout);
		IMethodResult CheckOutPhase(IElementIdentity phaseId);
	}
}