using System.Collections.Generic;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Implementations.Security
{
	public sealed class UserRepository : IUserRepository
	{
		public User CreateUser(User user)
		{
			throw new System.NotImplementedException();
		}

		public bool UpdateUserPassword(string username, string password)
		{
			throw new System.NotImplementedException();
		}
	}
}
