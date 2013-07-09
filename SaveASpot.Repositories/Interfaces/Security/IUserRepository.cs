using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface IUserRepository
	{
		User CreateUser(User user);
		bool UpdateUserPassword(string username, string password);
	}
}
