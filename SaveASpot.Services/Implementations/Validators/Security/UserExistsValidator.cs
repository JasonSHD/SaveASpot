using System.Linq;
using SaveASpot.Core.Validation;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UserExistsValidator : BaseValidator<UserArg>
	{
		private readonly IUserQueryable _userQueryable;

		public UserExistsValidator(IUserQueryable userQueryable)
			: base("UserNotExistsError")
		{
			_userQueryable = userQueryable;
		}

		protected override bool IsValid(UserArg input)
		{
			var userExists = _userQueryable.Filter(e => e.FilterByName(input.Username)).Find().Any();
			return userExists;
		}
	}
}