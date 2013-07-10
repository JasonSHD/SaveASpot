using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class UserHarvester : IUserHarvester
	{
		public UserViewModel Convert(User user)
		{
			return new UserViewModel(user.Username, user.Email);
		}

		public UserViewModel Anonyms()
		{
			return new UserViewModel(string.Empty, string.Empty);
		}

		public UserViewModel NotExists()
		{
			return new UserViewModel(string.Empty, string.Empty);
		}
	}
}