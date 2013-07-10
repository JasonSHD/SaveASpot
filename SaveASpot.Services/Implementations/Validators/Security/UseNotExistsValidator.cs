using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UseNotExistsValidator : IValidator<UserArg>
	{
		private readonly IUserQueryable _userQueryable;

		public UseNotExistsValidator(IUserQueryable userQueryable)
		{
			_userQueryable = userQueryable;
		}

		public IValidationResult Validate(UserArg input)
		{
			var userExists = _userQueryable.FindUsers(_userQueryable.FilterByName(input.Username)).Any();

			return userExists ? new ValidationResult(false, "UserExistsError") : new ValidationResult(true, string.Empty);
		}
	}
}