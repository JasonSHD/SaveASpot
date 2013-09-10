using SaveASpot.Core.Security;
using SaveASpot.Services.Interfaces.Controllers.Checkout;
using SaveASpot.ViewModels.Account;
using SaveASpot.ViewModels.Checkout;

namespace SaveASpot.Services.Implementations.Controllers.Checkout
{
	public sealed class UserInfoControllerService : IUserInfoControllerService
	{
		private readonly ICurrentUser _currentUser;

		public UserInfoControllerService(ICurrentUser currentUser)
		{
			_currentUser = currentUser;
		}

		public UserInfoViewModel UserInfo()
		{
			return new UserInfoViewModel { IsCustomer = _currentUser.User.IsCustomer(), LogOnViewModel = new LogOnViewModel(), User = _currentUser.User };
		}
	}
}