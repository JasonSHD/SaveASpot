using System.Collections.Generic;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface IUserQueryable
	{
		IUserFilter FilterByName(string name);
		IUserFilter FiltreByPassword(string password);
		IUserFilter FilterById(string identity);
		IUserFilter And(IUserFilter first, IUserFilter second);
		IEnumerable<UserEntity> FindUsers(IUserFilter userFilter);
	}
}