using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Services.Implementations.Controllers
{
	[CustomerAuthorize]
	public sealed class CustomerActionsMapControllerService : ICustomerActionsMapControllerService
	{
		private readonly ICurrentCustomer _currentCustomer;

		public CustomerActionsMapControllerService(ICurrentCustomer currentCustomer)
		{
			_currentCustomer = currentCustomer;
		}

		public bool BookingSpots(string[] identities)
		{
			throw new System.NotImplementedException();
		}
	}
}