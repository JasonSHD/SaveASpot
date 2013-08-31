using System.Linq;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class UserFactory : IUserFactory
	{
		private readonly IRoleFactory _roleFactory;

		public UserFactory(IRoleFactory roleFactory)
		{
			_roleFactory = roleFactory;
		}

		public User Convert(SiteUser siteUser)
		{
			return new User(siteUser.Identity, siteUser.Username, siteUser.Email, siteUser.Roles.Select(e => _roleFactory.Convert(e)));
		}

		public User AnonymUser()
		{
			return new User(string.Empty, string.Empty, string.Empty, new[] { _roleFactory.Convert(typeof(AnonymRole)) });
		}
	}
}