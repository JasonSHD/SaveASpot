using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class PasswordValidator : IValidator<UserArg>
	{
		public IValidationResult Validate(UserArg input)
		{
			var passwordValid = !string.IsNullOrWhiteSpace(input.Password) &&
			                    input.Password.Length >= RegisterViewModel.PasswordMinLength &&
			                    input.Password.Length <= RegisterViewModel.PasswordMaxLength;

			return passwordValid ? new ValidationResult(true, string.Empty) : new ValidationResult(false, string.Format("InvalidUserPasswordPassword", RegisterViewModel.PasswordMinLength, RegisterViewModel.PasswordMaxLength));
		}
	}
}