using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels.Security;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UseNotExistsValidator : IValidator<UserArg>
	{
		private readonly IUserService _userService;

		public UseNotExistsValidator(IUserService userService)
		{
			_userService = userService;
		}

		public IValidationResult Validate(UserArg input)
		{
			var userExists = _userService.GetUserByName(input.Username).IsExists();

			return userExists ? new ValidationResult(false, "UseExistsError") : new ValidationResult(true, string.Empty);
		}
	}
}