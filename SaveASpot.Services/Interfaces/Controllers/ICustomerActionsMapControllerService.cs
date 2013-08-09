namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ICustomerActionsMapControllerService
	{
		bool BookingSpots(string[] identities);
	}
}