using System.Web;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class CurrentUser : ICurrentUser
	{
		private readonly IUserService _userService;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public CurrentUser(IUserService userService, IElementIdentityConverter elementIdentityConverter)
		{
			_userService = userService;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public User User { get { return _userService.GetUserById(_elementIdentityConverter.ToIdentity(HttpContext.Current.User.Identity.Name)); } }
	}
}