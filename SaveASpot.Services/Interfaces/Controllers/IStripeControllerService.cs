using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IStripeControllerService
	{
		IMethodResult IsPaymentInformationAdded(string identityName);
		IMethodResult CreatePaymentInformation(string token, string identityName);
		IMethodResult CheckOutPhase(IElementIdentity phaseId, string spotPrice);
	}
}
