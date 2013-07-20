using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface IUserRepository
	{
		SiteUser CreateUser(SiteUser siteUser);
		bool UpdateUserPassword(string username, string password);
	}
}
