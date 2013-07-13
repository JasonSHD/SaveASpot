using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class SetupUsersControllerService : ISetupUsersControllerService
	{
		private readonly IUserService _userService;
		private readonly ITextService _textService;

		public SetupUsersControllerService(IUserService userService, ITextService textService)
		{
			_userService = userService;
			_textService = textService;
		}

		public IMethodResult<MessageResult> AdminExists()
		{
			if (_userService.GetByRole(new AdministratorRole()).Any())
			{
				return new MessageMethodResult(true, _textService.ResolveTest("AdminExists"));
			}

			return new MessageMethodResult(false, string.Empty);
		}

		public IMethodResult<MessageResult> RegisterAdmin(RegisterViewModel registerViewModel)
		{
			var createUserResult =
				_userService.CreateUser(new UserArg
																	{
																		Email = registerViewModel.Email,
																		Password = registerViewModel.Password,
																		Username = registerViewModel.UserName
																	}, new[] { new AdministratorRole() });

			return new MessageMethodResult(createUserResult.IsSuccess, _textService.ResolveTest(createUserResult.Status.MessageKet));
		}
	}
}