using SaveASpot.Core.Validation;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class PasswordValidator : CompositeValidator
	{
		public PasswordValidator()
			: base(Validator.For<UserArg>().
							For(e => e.Password, e => e.Lenght(Constants.PasswordMinLength, Constants.PasswordMaxLength, "InvalidUserPasswordPassword")))
		{
		}
	}
}