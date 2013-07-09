using SaveASpot.Core;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IUserService
	{
		IMethodResult<CreateUserResult> CreateUser(LogOnViewModel logOnViewModel);
	}

	public sealed class CreateUserResult { }
}
