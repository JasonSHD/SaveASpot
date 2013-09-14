using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ICardInfoControllerService
	{
		IMethodResult IsPaymentInformationAdded();
		IMethodResult CreatePaymentInformation(string token);
	}
}
