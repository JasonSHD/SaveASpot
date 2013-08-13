using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Core.Security;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IUserService
	{
		IMethodResult<UserExistsResult> UserExists(string username, string password);
		IMethodResult<CreateUserResult> CreateUser(UserArg userArg, IEnumerable<Role> roles);
		IMethodResult<MessageResult> ChangePassword(string username, string newPassword);
		User GetUserById(IElementIdentity id);
		User GetUserByName(string username);
		IEnumerable<User> GetByRole(Role role);
	}
}
