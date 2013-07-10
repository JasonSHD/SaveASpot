using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UserExistsValidator : IValidator<UserArg>
	{
		private readonly IUserQueryable _userQueryable;

		public UserExistsValidator(IUserQueryable userQueryable)
		{
			_userQueryable = userQueryable;
		}

		public IValidationResult Validate(UserArg input)
		{
			var userExists = _userQueryable.FindUsers(_userQueryable.FilterByName(input.Username)).Any();
			return userExists ? new ValidationResult(true, string.Empty) : new ValidationResult(false, "UserNotExistsError");
		}
	}
}