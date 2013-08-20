using SaveASpot.Core;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IAdministratorActionsMapControllerService
	{
		BookingSpotsViewModel BookingSpots(IElementIdentity[] identities, IElementIdentity sponsorIdentity);
	}
}