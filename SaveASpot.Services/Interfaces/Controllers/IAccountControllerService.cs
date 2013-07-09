using SaveASpot.Core;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IAccountControllerService
	{
		IMethodResult<MessageResult> LogOn(LogOnViewModel logOnViewModel);
		IMethodResult<MessageResult> RegisterUser(RegisterViewModel registerViewModel);
		IMethodResult<MessageResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel);
	}
}
