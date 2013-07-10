using SaveASpot.Core;
using SaveASpot.ViewModels.Security;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IUserService
	{
		IMethodResult<UserExistsResult> UserExists(string username, string password);
		IMethodResult<CreateUserResult> CreateUser(UserArg userArg);
		IMethodResult<MessageResult> ChangePassword(string username, string newPassword);
		User GetUserById(string id);
		User GetUserByName(string username);
	}
}
