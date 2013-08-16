using SaveASpot.Core;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ICustomerActionsMapControllerService
	{
		BookingSpotsViewModel BookingSpots(IElementIdentity[] identities);
		BookingSpotsViewModel RemoveBookedSpot(IElementIdentity bookedSpotIdentity);
	}
}