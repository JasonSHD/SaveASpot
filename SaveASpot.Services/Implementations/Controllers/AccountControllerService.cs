using SaveASpot.Core;
using SaveASpot.Core.Web;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Interfaces.Security;
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
		private readonly IUserFactory _userFactory;
		private readonly IUserQueryable _userQueryable;

		public AccountControllerService(IUserService userService, IWebAuthentication webAuthentication, ITextService textService, IUserFactory userFactory, IUserQueryable userQueryable)
		{
			_userService = userService;
			_webAuthentication = webAuthentication;
			_textService = textService;
			_userFactory = userFactory;
			_userQueryable = userQueryable;
		}

		public IMethodResult<UserResult> LogOn(LogOnViewModel logOnViewModel)
		{
			var userExistsResult = _userService.UserExists(logOnViewModel.UserName, logOnViewModel.Password);

			if (userExistsResult.IsSuccess)
			{
				_webAuthentication.Authenticate(userExistsResult.Status.UserId, logOnViewModel.RememberMe);

				var user =
					_userFactory.Convert(_userQueryable.Filter(e => e.FilterById(userExistsResult.Status.UserId)).First());

				return new MethodResult<UserResult>(true, new UserResult(user, string.Empty));
			}

			return new MethodResult<UserResult>(false, new UserResult(_userFactory.NotExists(), _textService.ResolveTest(userExistsResult.Status.MessageKey)));
		}

		public IMethodResult<MessageResult> LogOff()
		{
			_webAuthentication.LogOff();

			return new MessageMethodResult(true, string.Empty);
		}
	}
}
