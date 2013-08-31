using System;
using System.Linq;
using SaveASpot.Core.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class RoleFactory : IRoleFactory
	{
		private readonly Role[] _roles;

		public RoleFactory()
		{
			_roles = new Role[] { new AdministratorRole(), new CustomerRole(), new AnonymRole() };
		}

		public Role Convert(string identity)
		{
			return _roles.First(e => e.Identity == identity);
		}

		public Role Convert(Type role)
		{
			return _roles.First(e => e.GetType() == role);
		}
	}
}