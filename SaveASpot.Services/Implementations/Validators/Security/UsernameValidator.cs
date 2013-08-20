using SaveASpot.Core.Validation;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UsernameValidator : CompositeValidator
	{
		public UsernameValidator()
			: base(Validator.For<UserArg>().For(e => e.Username, e => e.Required().StringRequired()))
		{
		}
	}
}