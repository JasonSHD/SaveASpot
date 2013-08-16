using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels;

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

		public BookingSpotsViewModel BookingSpots(IElementIdentity[] identities)
		{
			return new BookingSpotsViewModel
							 {
								 BookedSpots =
									 identities.ToList()
														 .Where(
															 identity =>
															 _spotsBookingService.BookingForCustomer(identity, _currentCustomer.Customer.Identity))
														 .ToList(),
								 Cart = _currentCustomer.Customer.Cart
							 };
		}

		public BookingSpotsViewModel RemoveBookedSpot(IElementIdentity bookedSpotIdentity)
		{
			return new BookingSpotsViewModel
							 {
								 BookedSpots =
									 _spotsBookingService.RemoveBookedSpot(bookedSpotIdentity, _currentCustomer.Customer.Identity)
										 ? new[] { bookedSpotIdentity }
										 : Enumerable.Empty<IElementIdentity>(),
								 Cart = _currentCustomer.Customer.Cart
							 };
		}
	}
}