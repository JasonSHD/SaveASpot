using SaveASpot.Core;
using SaveASpot.ViewModels.Checkout;

namespace SaveASpot.Services.Interfaces.Controllers.Checkout
{
	public interface ISpotsControllerService
	{
		SpotsCheckoutViewModel GetSpots(IElementIdentity phaseIdentity);
	}
}
