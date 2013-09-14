using System.Collections.Generic;

namespace SaveASpot.Core.Security
{
	public sealed class User
	{
		private readonly IElementIdentity _identity;
		private readonly string _name;
		private readonly string _email;
		private readonly IEnumerable<Role> _roles;

		public User(IElementIdentity identity, string name, string email, IEnumerable<Role> roles)
		{
			_identity = identity;
			_name = name;
			_email = email;
			_roles = roles;
		}

		public IElementIdentity Identity { get { return _identity; } }
		public string Name { get { return _name; } }
		public string Email { get { return _email; } }
		public IEnumerable<Role> Roles { get { return _roles; } }
	}
}
