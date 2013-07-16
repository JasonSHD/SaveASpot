using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class UserHarvester : IUserHarvester
	{
		private readonly IEnumerable<Role> _roles;

		public UserHarvester()
		{
			_roles = new Role[] { new AdministratorRole(), new CustomerRole() };
		}

		public User Convert(SiteUser siteUser)
		{
			return new User(siteUser.Identity, siteUser.Username, siteUser.Email, siteUser.Roles.Select(e => _roles.First(r => r.Identity == e)));
		}

		public User NotExists()
		{
			return new User(string.Empty, string.Empty, string.Empty, new Role[0]);
		}
	}
}