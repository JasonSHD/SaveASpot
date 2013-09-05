using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces
{
	public interface ISpotsBookingService
	{
		bool BookingForSponsor(IElementIdentity spotIdentity, IElementIdentity sponsorIdentity);
	}
}