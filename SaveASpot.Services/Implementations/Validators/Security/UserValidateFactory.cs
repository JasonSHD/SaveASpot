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

		public IValidator UserExistsValidator()
		{
			return new UserExistsValidator(_userQueryable);
		}

		public IValidator UserNotExistsValidator()
		{
			return new UseNotExistsValidator(_userQueryable);
		}

		public IValidator UserNameValidator()
		{
			return new UsernameValidator();
		}

		public IValidator PasswordValidator()
		{
			return new PasswordValidator();
		}

		public IValidator EmailValidator()
		{
			return new UserEmailValidator();
		}
	}
}
