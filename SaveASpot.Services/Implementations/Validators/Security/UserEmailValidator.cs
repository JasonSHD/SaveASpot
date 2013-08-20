using SaveASpot.Core.Validation;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UserEmailValidator : CompositeValidator
	{
		public UserEmailValidator()
			: base(Validator.
				For<UserArg>().
				For(e => e.Email, e => e.StringRequired().StringRequired().Regex(Constants.EmailRegularExpression, "InvalidUserEmail")))
		{
		}
	}
}