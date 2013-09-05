using SaveASpot.Core.Security;
using SaveASpot.Core.Web;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class CurrentUser : ICurrentUser
	{
		private readonly IUserService _userService;
		private readonly ICurrentSessionIdentity _currentSessionIdentity;

		public CurrentUser(IUserService userService, ICurrentSessionIdentity currentSessionIdentity)
		{
			_userService = userService;
			_currentSessionIdentity = currentSessionIdentity;
		}

		public User User { get { return _userService.GetUserById(_currentSessionIdentity.UserIdentity); } }
	}
}