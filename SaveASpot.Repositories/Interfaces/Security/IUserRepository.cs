using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface IUserRepository
	{
		UserEntity CreateUser(UserEntity userEntity);
		bool UpdateUserPassword(string username, string password);
	}
}
