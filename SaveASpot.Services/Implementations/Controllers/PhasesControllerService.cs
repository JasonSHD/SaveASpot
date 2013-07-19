using System.Collections.Generic;
using System.Linq;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class PhasesAndParcelsControllerService : IPhasesAndParcelsControllerService
	{
		public IEnumerable<PhaseViewModel> GetPhases()
		{
			var result = new List<PhaseViewModel>();

			for (var phaseIndex = 0; phaseIndex < 10; phaseIndex++)
			{
				var phase = new PhaseViewModel { Name = "Phase #" + phaseIndex };
				result.Add(phase);
			}

			return result;
		}
	}

	public sealed class ParcelsControllerService : IParcelsControllerService
	{
		public IEnumerable<ParcelViewModel> GetParcels()
		{
			return Enumerable.Empty<ParcelViewModel>();
		}
	}
}