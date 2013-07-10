using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UsernameValidator : IValidator<UserArg>
	{
		public IValidationResult Validate(UserArg input)
		{
			var nameValid = !string.IsNullOrWhiteSpace(input.Username) && input.Username.Length > 0;

			return nameValid ? new ValidationResult(true, string.Empty) : new ValidationResult(false, "InvalidUserName");
		}
	}
}