using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels.Account;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class AccountControllerService : IAccountControllerService
	{
		private readonly IWebAuthentication _webAuthentication;
		private readonly ITextService _textService;
		private readonly IUserFactory _userFactory;
		private readonly IRoleFactory _roleFactory;
		private readonly IUserQueryable _userQueryable;
		private readonly IPasswordHash _passwordHash;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public AccountControllerService(IWebAuthentication webAuthentication,
			ITextService textService,
			IUserFactory userFactory,
			IRoleFactory roleFactory,
			IUserQueryable userQueryable,
			IPasswordHash passwordHash,
			IElementIdentityConverter elementIdentityConverter)
		{
			_webAuthentication = webAuthentication;
			_textService = textService;
			_userFactory = userFactory;
			_roleFactory = roleFactory;
			_userQueryable = userQueryable;
			_passwordHash = passwordHash;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public IMethodResult<UserResult> LogOff()
		{
			_webAuthentication.LogOff();

			return new MethodResult<UserResult>(true, new UserResult(_userFactory.AnonymUser(), string.Empty));
		}

		public LogOnResultViewModel LogOnAdmin(LogOnViewModel logOn)
		{
			var user = _userQueryable.
				Filter(e => e.FilterByName(logOn.UserName)).
				And(e => e.FilterByPassword(_passwordHash.GetHash(logOn.Password, logOn.UserName))).
				And(e => e.FilterByRole(_roleFactory.Convert(typeof(AdministratorRole)))).
				FirstOrDefault();

			if (user != null)
			{
				_webAuthentication.Authenticate(_elementIdentityConverter.ToIdentity(user.Id), logOn.RememberMe);

				return new LogOnResultViewModel(true, string.Empty, _userFactory.Convert(user));
			}

			return new LogOnResultViewModel(false, _textService.ResolveTest("UserNotExistsError"), _userFactory.AnonymUser());
		}

		public LogOnResultViewModel LogOnCustomer(LogOnViewModel logOn)
		{
			throw new System.NotImplementedException();
		}
	}
}
