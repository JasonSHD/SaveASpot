using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class AccountControllerService : IAccountControllerService
	{
		private readonly IUserService _userService;
		private readonly IWebAuthentication _webAuthentication;
		private readonly ITextService _textService;
		private readonly ICurrentUser _currentUser;
		private readonly IUserHarvester _userHarvester;
		private readonly IUserQueryable _userQueryable;

		public AccountControllerService(IUserService userService, IWebAuthentication webAuthentication, ITextService textService, ICurrentUser currentUser, IUserHarvester userHarvester, IUserQueryable userQueryable)
		{
			_userService = userService;
			_webAuthentication = webAuthentication;
			_textService = textService;
			_currentUser = currentUser;
			_userHarvester = userHarvester;
			_userQueryable = userQueryable;
		}

		public IMethodResult<UserResult> LogOn(LogOnViewModel logOnViewModel)
		{
			var userExistsResult = _userService.UserExists(logOnViewModel.UserName, logOnViewModel.Password);

			if (userExistsResult.IsSuccess)
			{
				_webAuthentication.Authenticate(userExistsResult.Status.UserId, logOnViewModel.RememberMe);
				var user =
					_userHarvester.Convert(_userQueryable.FindUsers(_userQueryable.FilterById(userExistsResult.Status.UserId)).First());

				return new MethodResult<UserResult>(true, new UserResult(user, string.Empty));
			}

			return new MethodResult<UserResult>(false, new UserResult(_userHarvester.NotExists(), _textService.ResolveTest(userExistsResult.Status.MessageKey)));
		}

		public IMethodResult<MessageResult> RegisterUser(RegisterViewModel registerViewModel)
		{
			var createUserResult = _userService.CreateUser(new UserArg { Email = registerViewModel.Email, Password = registerViewModel.Password, Username = registerViewModel.UserName }, new[] { new CreatorRole(), });

			return new MessageMethodResult(createUserResult.IsSuccess, _textService.ResolveTest(createUserResult.Status.MessageKet));
		}

		public IMethodResult<MessageResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
		{
			var userExists = _userService.UserExists(_currentUser.User.Name, changePasswordViewModel.OldPassword);

			if (userExists.IsSuccess)
			{
				var changePasswordResult = _userService.ChangePassword(_currentUser.User.Name,
																															 changePasswordViewModel.NewPassword);

				if (changePasswordResult.IsSuccess)
				{
					return new MessageMethodResult(true, string.Empty);
				}
			}

			return new MessageMethodResult(false, _textService.ResolveTest("InvalidOldOrNewPassword"));
		}

		public IMethodResult<MessageResult> LogOff()
		{
			_webAuthentication.LogOff();

			return new MessageMethodResult(true, string.Empty);
		}
	}
}
