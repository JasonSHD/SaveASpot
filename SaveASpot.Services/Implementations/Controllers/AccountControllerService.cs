using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class AccountControllerService : IAccountControllerService
	{
		public IMethodResult<CreateUserResult> LogOn(LogOnViewModel logOnViewModel)
		{
			throw new System.NotImplementedException();
		}
	}
}
