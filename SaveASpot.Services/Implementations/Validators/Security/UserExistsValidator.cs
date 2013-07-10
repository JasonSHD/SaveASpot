using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels.Security;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UserExistsValidator : IValidator<UserArg>
	{
		private readonly IUserService _userService;

		public UserExistsValidator(IUserService userService)
		{
			_userService = userService;
		}

		public IValidationResult Validate(UserArg input)
		{
			var userExists = _userService.GetUserByName(input.Username).IsExists();
			return userExists ? new ValidationResult(true, string.Empty) : new ValidationResult(false, "UserNotExistsError");
		}
	}
}