using SaveASpot.Core;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ISetupUsersControllerService
	{
		IMethodResult<MessageResult> AdminExists();
		IMethodResult<MessageResult> RegisterAdmin(RegisterViewModel registerViewModel);
	}
}