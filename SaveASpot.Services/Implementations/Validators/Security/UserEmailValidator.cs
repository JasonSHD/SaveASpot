using System.Text.RegularExpressions;
using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UserEmailValidator : IValidator<UserArg>
	{
		public IValidationResult Validate(UserArg input)
		{
			var emailRegex = new Regex(Constants.EmailRegularExpression);
			var emailIsValid = emailRegex.IsMatch(input.Email);

			return emailIsValid ? new ValidationResult(true, string.Empty) : new ValidationResult(false, "InvalidUserEmail");
		}
	}
}