using SaveASpot.Core.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface IUserQueryable : IElementQueryable<SiteUser, IUserFilter>
	{
		IUserFilter FilterByName(string name);
		IUserFilter FilterByPassword(string password);
		IUserFilter FilterByRole(Role role);
		IUserFilter FilterById(string identity);
		IUserFilter FilterByIds(string[] identities);
	}
}