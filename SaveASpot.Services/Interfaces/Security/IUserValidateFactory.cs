using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IUserValidateFactory
	{
		IValidator<UserArg> UserExistsValidator();
		IValidator<UserArg> UserNotExistsValidator();
		IValidator<UserArg> UserNameValidator();
		IValidator<UserArg> PasswordValidator();
		IValidator<UserArg> EmailValidator();
	}
}