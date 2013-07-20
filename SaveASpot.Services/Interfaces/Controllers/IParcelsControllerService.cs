using System.Collections.Generic;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IParcelsControllerService
	{
		IEnumerable<ParcelViewModel> GetParcels();
	}
}