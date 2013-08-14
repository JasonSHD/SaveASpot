using System.Collections.Generic;
using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ICustomerActionsMapControllerService
	{
		IEnumerable<IElementIdentity> BookingSpots(IElementIdentity[] identities);
	}
}