using System.Web;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels.Security;

namespace SaveASpot.Services.Implementations
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