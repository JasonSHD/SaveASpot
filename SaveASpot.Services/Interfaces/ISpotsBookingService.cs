using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces
{
	public interface ISpotsBookingService
	{
		bool BookingForCustomer(IElementIdentity spotIdentity, IElementIdentity customerIdentity);
		bool RemoveBookedSpot(IElementIdentity bookedSpotIdentity, IElementIdentity customerIdentity);
	}
}