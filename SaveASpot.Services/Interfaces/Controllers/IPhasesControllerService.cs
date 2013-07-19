using System.Collections.Generic;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IPhasesAndParcelsControllerService
	{
		IEnumerable<PhaseViewModel> GetPhases();
	}

	public sealed class PhaseViewModel
	{
		public string Name { get; set; }
		//public IEnumerable<ParcelViewModel> Parcels { get; set; }
	}

	public sealed class ParcelViewModel
	{
		public string Name { get; set; }
		//public IEnumerable<SpotViewModel> Spots { get; set; }
	}

	public sealed class SpotViewModel
	{
		public string Name { get; set; }
	}
}