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
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public CustomerActionsMapControllerService(ICurrentCustomer currentCustomer, ISpotsBookingService spotsBookingService, IElementIdentityConverter elementIdentityConverter)
		{
			_currentCustomer = currentCustomer;
			_spotsBookingService = spotsBookingService;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public bool BookingSpots(string[] identities)
		{
			identities.ToList().ForEach(identity => _spotsBookingService.BookingForCustomer(_elementIdentityConverter.ToIdentity(identity), _elementIdentityConverter.ToIdentity(_currentCustomer.Customer.Identity)));

			return false;
		}
	}
}