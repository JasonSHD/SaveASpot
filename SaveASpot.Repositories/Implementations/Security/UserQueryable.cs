using System.Collections.Generic;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Implementations.Security
{
	public sealed class UserQueryable : IUserQueryable
	{
		public IUserFilter FilterByName(string name)
		{
			throw new System.NotImplementedException();
		}

		public IUserFilter FiltreByPassword(string password)
		{
			throw new System.NotImplementedException();
		}

		public IUserFilter FilterById(string identity)
		{
			throw new System.NotImplementedException();
		}

		public IUserFilter And(IUserFilter first, IUserFilter second)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<User> FindUsers(IUserFilter userFilter)
		{
			throw new System.NotImplementedException();
		}
	}

	public sealed class UserFilter : IUserFilter
	{

	}
}