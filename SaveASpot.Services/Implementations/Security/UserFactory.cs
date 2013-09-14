using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class UserFactory : IUserFactory, IAnonymUser
	{
		private readonly IRoleFactory _roleFactory;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public UserFactory(IRoleFactory roleFactory, IElementIdentityConverter elementIdentityConverter)
		{
			_roleFactory = roleFactory;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public User Convert(SiteUser siteUser)
		{
			return new User(_elementIdentityConverter.ToIdentity(siteUser.Identity), siteUser.Username, siteUser.Email, siteUser.Roles.Select(e => _roleFactory.Convert(e)));
		}

		public User AnonymUser()
		{
			return new User(new NullElementIdentity(), string.Empty, string.Empty, new[] { _roleFactory.Convert(typeof(AnonymRole)) });
		}

		public User User { get { return AnonymUser(); } }
	}
}