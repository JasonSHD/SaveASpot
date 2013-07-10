using System.Collections.Generic;

namespace SaveASpot.ViewModels.Security
{
	public sealed class User
	{
		private readonly string _identity;
		private readonly string _name;
		private readonly string _email;
		private readonly IEnumerable<Role> _roles;

		public User(string identity, string name, string email, IEnumerable<Role> roles)
		{
			_identity = identity;
			_name = name;
			_email = email;
			_roles = roles;
		}

		public string Identity { get { return _identity; } }
		public string Name { get { return _name; } }
		public string Email { get { return _email; } }
		public IEnumerable<Role> Roles { get { return _roles; } }
	}
}
