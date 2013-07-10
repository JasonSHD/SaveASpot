using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UserValidateFactory : IUserValidateFactory
	{
		private readonly IUserService _userService;

		public UserValidateFactory(IUserService userService)
		{
			_userService = userService;
		}

		public IValidator<UserArg> UserExistsValidator()
		{
			return new UserExistsValidator(_userService);
		}

		public IValidator<UserArg> UserNotExistsValidator()
		{
			return new UseNotExistsValidator(_userService);
		}

		public IValidator<UserArg> UserNameValidator()
		{
			return new UsernameValidator();
		}

		public IValidator<UserArg> PasswordValidator()
		{
			return new PasswordValidator();
		}

		public IValidator<UserArg> EmailValidator()
		{
			return new UserEmailValidator();
		}
	}
}
