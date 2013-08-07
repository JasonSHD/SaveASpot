using SaveASpot.Core.Security;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class MapControllerService : IMapControllerService
	{
		private readonly ICurrentUser _currentUser;
		private readonly IRoleFactory _roleFactory;

		public MapControllerService(ICurrentUser currentUser, IRoleFactory roleFactory)
		{
			_currentUser = currentUser;
			_roleFactory = roleFactory;
		}

		public MapViewModel GetMapViewModel()
		{
			var customerRole = _roleFactory.Convert(typeof(CustomerRole));

			return new MapViewModel
							 {
								 IsCustomer = _currentUser.User.IsCustomer(customerRole)
							 };
		}
	}
}