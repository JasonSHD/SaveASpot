using SaveASpot.Core;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IAccountControllerService
	{
		IMethodResult<UserResult> LogOn(LogOnViewModel logOnViewModel);
		IMethodResult<UserResult> LogOff();
	}
}
