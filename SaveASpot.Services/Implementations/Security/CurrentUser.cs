using System.Web;
using SaveASpot.Core.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class CurrentUser : ICurrentUser
	{
		private readonly IUserService _userService;

		public CurrentUser(IUserService userService)
		{
			_userService = userService;
		}

		public User User { get { return _userService.GetUserById(HttpContext.Current.User.Identity.Name); } }
	}
}