using SaveASpot.Core;
using SaveASpot.ViewModels.Account;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IAccountControllerService
	{
		IMethodResult<UserResult> LogOff();
		LogOnResultViewModel LogOnAdmin(LogOnViewModel logOn);
		LogOnResultViewModel LogOnCustomer(LogOnViewModel logOn);
	}
}
