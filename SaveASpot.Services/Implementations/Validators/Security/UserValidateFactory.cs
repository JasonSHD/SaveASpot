using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UserValidateFactory : IUserValidateFactory
	{
		private readonly IUserQueryable _userQueryable;

		public UserValidateFactory(IUserQueryable userQueryable)
		{
			_userQueryable = userQueryable;
		}

		public IValidator<UserArg> UserExistsValidator()
		{
			return new UserExistsValidator(_userQueryable);
		}

		public IValidator<UserArg> UserNotExistsValidator()
		{
			return new UseNotExistsValidator(_userQueryable);
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
