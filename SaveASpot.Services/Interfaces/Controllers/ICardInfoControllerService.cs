using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ICardInfoControllerService
	{
		IMethodResult CreatePaymentInformation(string token);
	}
}
