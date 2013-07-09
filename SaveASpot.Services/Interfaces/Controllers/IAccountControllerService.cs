using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IAccountControllerService
	{
		IMethodResult<CreateUserResult> LogOn(LogOnModel logOnModel);
	}
}
