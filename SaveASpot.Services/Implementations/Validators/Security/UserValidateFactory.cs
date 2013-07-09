using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UserValidateFactory : IUserValidateFactory
	{
		public IValidator<UserArg> UserExistsValidator()
		{
			throw new System.NotImplementedException();
		}

		public IValidator<UserArg> UserNotExistsValidator()
		{
			throw new System.NotImplementedException();
		}

		public IValidator<UserArg> UserNameValidator()
		{
			throw new System.NotImplementedException();
		}

		public IValidator<UserArg> PasswordValidator()
		{
			throw new System.NotImplementedException();
		}

		public IValidator<UserArg> EmailValidator()
		{
			throw new System.NotImplementedException();
		}
	}
}
