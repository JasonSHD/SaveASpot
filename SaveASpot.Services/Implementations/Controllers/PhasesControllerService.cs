using System.Collections.Generic;
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
}