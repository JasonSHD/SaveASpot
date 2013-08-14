using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class CustomerActionsMapControllerService : ICustomerActionsMapControllerService
	{
		private readonly ICurrentCustomer _currentCustomer;
		private readonly ISpotsBookingService _spotsBookingService;

		public CustomerActionsMapControllerService(ICurrentCustomer currentCustomer, ISpotsBookingService spotsBookingService)
		{
			_currentCustomer = currentCustomer;
			_spotsBookingService = spotsBookingService;
		}

		public IEnumerable<IElementIdentity> BookingSpots(IElementIdentity[] identities)
		{
			return identities.ToList().Where(identity => _spotsBookingService.BookingForCustomer(identity, _currentCustomer.Customer.Identity)).ToList();
		}
	}
}