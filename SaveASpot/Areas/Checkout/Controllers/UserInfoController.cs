using System.Web.Mvc;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers.Checkout;

namespace SaveASpot.Areas.Checkout.Controllers
{
	public sealed class UserInfoController : BaseController
	{
		private readonly IUserInfoControllerService _userInfoControllerService;

		public UserInfoController(IUserInfoControllerService userInfoControllerService)
		{
			_userInfoControllerService = userInfoControllerService;
		}

		[HttpGet]
		public ActionResult UserInfo()
		{
			var userInfoResult = _userInfoControllerService.UserInfo();

			return userInfoResult.IsCustomer ? View("AuthenticatedUser", userInfoResult.User) : View(userInfoResult.LogOnViewModel);
		}
	}
}