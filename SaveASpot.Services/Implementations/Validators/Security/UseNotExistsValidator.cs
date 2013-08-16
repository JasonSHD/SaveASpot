using System.Linq;
using SaveASpot.Core.Validation;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UseNotExistsValidator : BaseValidator<UserArg>
	{
		private readonly IUserQueryable _userQueryable;

		public UseNotExistsValidator(IUserQueryable userQueryable)
			: base("UserExistsError")
		{
			_userQueryable = userQueryable;
		}

		protected override bool IsValid(UserArg input)
		{
			return !_userQueryable.Filter(e => e.FilterByName(input.Username)).Find().Any();
		}
	}
}