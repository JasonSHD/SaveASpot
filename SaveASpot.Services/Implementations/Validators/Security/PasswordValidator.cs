using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class PasswordValidator : IValidator<UserArg>
	{
		public IValidationResult Validate(UserArg input)
		{
			int passwordMinLength = Constants.PasswordMinLength;
			int passwordMaxLength = Constants.PasswordMaxLength;
			var passwordValid = !string.IsNullOrWhiteSpace(input.Password) &&
			                    input.Password.Length >= passwordMinLength &&
			                    input.Password.Length <= passwordMaxLength;

			return passwordValid ? new ValidationResult(true, string.Empty) : new ValidationResult(false, string.Format("InvalidUserPasswordPassword", passwordMinLength, passwordMaxLength));
		}
	}
}