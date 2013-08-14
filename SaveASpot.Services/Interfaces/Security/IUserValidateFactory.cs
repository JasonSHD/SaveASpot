using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IUserValidateFactory
	{
		IValidator UserExistsValidator();
		IValidator UserNotExistsValidator();
		IValidator UserNameValidator();
		IValidator PasswordValidator();
		IValidator EmailValidator();
	}
}