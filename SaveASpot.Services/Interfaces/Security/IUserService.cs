using SaveASpot.Core;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IUserService
	{
		IMethodResult<UserExistsResult> UserExists(string username, string password);
		IMethodResult<CreateUserResult> CreateUser(UserArg userArg);
		IMethodResult<MessageResult> ChangePassword(string username, string newPassword);
		UserViewModel GetUserById(string id);
	}
}
