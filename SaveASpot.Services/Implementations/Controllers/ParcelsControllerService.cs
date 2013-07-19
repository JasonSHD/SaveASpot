using System.Collections.Generic;
using System.Linq;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class ParcelsControllerService : IParcelsControllerService
	{
		public IEnumerable<ParcelViewModel> GetParcels()
		{
			return Enumerable.Empty<ParcelViewModel>();
		}
	}
}