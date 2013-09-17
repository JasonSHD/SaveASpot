using System.Linq;
using SaveASpot.Core.Validation;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Validators.Security
{
	public sealed class UserExistsValidator : BaseValidator<UserArg>
	{
		private readonly IUserQueryable _userQueryable;

		public UserExistsValidator(IUserQueryable userQueryable)
			: base(Constants.Errors.UserNotExistsError)
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