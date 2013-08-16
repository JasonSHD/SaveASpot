using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces
{
	public interface ISpotsBookingService
	{
		bool BookingForCustomer(IElementIdentity spotIdentity, IElementIdentity customerIdentity);
		bool BookingForSponsor(IElementIdentity spotIdentity, IElementIdentity sponsorIdentity);
		bool RemoveBookedSpot(IElementIdentity bookedSpotIdentity, IElementIdentity customerIdentity);
	}
}