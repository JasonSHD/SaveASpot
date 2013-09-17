using System;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;
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
		private readonly ICustomerService _customerService;
		private readonly ICustomerQueryable _customerQueryable;
		private readonly ICustomerFactory _customerFactory;

		public AccountControllerService(IWebAuthentication webAuthentication,
			ITextService textService,
			IUserFactory userFactory,
			IRoleFactory roleFactory,
			IUserQueryable userQueryable,
			IPasswordHash passwordHash,
			IElementIdentityConverter elementIdentityConverter,
			ICustomerService customerService,
			ICustomerQueryable customerQueryable,
			ICustomerFactory customerFactory)
		{
			_webAuthentication = webAuthentication;
			_textService = textService;
			_userFactory = userFactory;
			_roleFactory = roleFactory;
			_userQueryable = userQueryable;
			_passwordHash = passwordHash;
			_elementIdentityConverter = elementIdentityConverter;
			_customerService = customerService;
			_customerQueryable = customerQueryable;
			_customerFactory = customerFactory;
		}

		public IMethodResult<UserResult> LogOff()
		{
			_webAuthentication.LogOff();

			return new MethodResult<UserResult>(true, new UserResult(_userFactory.AnonymUser(), string.Empty));
		}

		public LogOnResultViewModel LogOnAdmin(LogOnViewModel logOn)
		{
			var longOnUserResult = LogOnUser(logOn, typeof(AdministratorRole));
			return new LogOnResultViewModel(longOnUserResult.IsSuccess, longOnUserResult.IsSuccess ? string.Empty : _textService.ResolveTest(Constants.Errors.UserNotExistsError), longOnUserResult.Status);
		}

		public LogOnCustomerResultViewModel LogOnCustomer(LogOnViewModel logOn)
		{
			var logOnUserResult = LogOnUser(logOn, typeof(CustomerRole));
			if (logOnUserResult.IsSuccess)
			{
				var customer = _customerQueryable.Filter(e => e.FilterByUserId(logOnUserResult.Status.Identity)).First();
				return new LogOnCustomerResultViewModel(logOnUserResult.IsSuccess, string.Empty, _customerFactory.Convert(logOnUserResult.Status, customer));
			}

			return new LogOnCustomerResultViewModel(logOnUserResult.IsSuccess, _textService.ResolveTest(Constants.Errors.UserNotExistsError), _customerFactory.NotCustomer());
		}

		public LogOnResultViewModel RegistrateCustomer(CreateCustomerViewModel createCustomerViewModel)
		{
			var result = _customerService.CreateCustomer(
				new UserArg
					{
						Email = createCustomerViewModel.Email,
						Password = createCustomerViewModel.Password,
						Username = createCustomerViewModel.UserName
					});

			if (result.IsSuccess)
			{
				var longOnUserResult = LogOnUser(new LogOnViewModel
																					 {
																						 Password = createCustomerViewModel.Password,
																						 RememberMe = false,
																						 UserName = createCustomerViewModel.UserName
																					 }, typeof(CustomerRole));
				return new LogOnResultViewModel(longOnUserResult.IsSuccess, longOnUserResult.IsSuccess ? string.Empty : _textService.ResolveTest(Constants.Errors.UserNotExistsError), longOnUserResult.Status);
			}

			return new LogOnResultViewModel(false, string.Empty, _userFactory.AnonymUser());
		}

		private IMethodResult<User> LogOnUser(LogOnViewModel logOn, Type roleType)
		{
			var user = _userQueryable.
				Filter(e => e.FilterByName(logOn.UserName)).
				And(e => e.FilterByPassword(_passwordHash.GetHash(logOn.Password, logOn.UserName))).
				And(e => e.FilterByRole(_roleFactory.Convert(roleType))).
				FirstOrDefault();

			if (user != null)
			{
				_webAuthentication.Authenticate(_elementIdentityConverter.ToIdentity(user.Id), logOn.RememberMe);

				return new MethodResult<User>(true, _userFactory.Convert(user));
			}

			return new MethodResult<User>(false, _userFactory.AnonymUser());
		}
	}
}
