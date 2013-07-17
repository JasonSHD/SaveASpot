using System.Collections.Generic;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IParcelsControllerService
	{
		IEnumerable<ParcelViewModel> GetParcels();
	}
}