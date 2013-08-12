using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ICustomerActionsMapControllerService
	{
		bool BookingSpots(IElementIdentity[] identities);
	}
}