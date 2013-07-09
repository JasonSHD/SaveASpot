using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IUserService
	{
		IMethodResult<CreateUserResult> CreateUser();
	}

	public sealed class CreateUserResult { }
}
