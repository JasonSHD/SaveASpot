using System.Linq;
using SaveASpot.Core;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class AdministratorActionsMapControllerService : IAdministratorActionsMapControllerService
	{
		private readonly ISpotsBookingService _spotsBookingService;

		public AdministratorActionsMapControllerService(ISpotsBookingService spotsBookingService)
		{
			_spotsBookingService = spotsBookingService;
		}

		public BookingSpotsViewModel BookingSpots(IElementIdentity[] identities, IElementIdentity sponsorIdentity)
		{
			return new BookingSpotsViewModel
							 {
								 BookedSpots = identities.Where(e => _spotsBookingService.BookingForSponsor(e, sponsorIdentity)).ToList()
							 };
		}
	}
}