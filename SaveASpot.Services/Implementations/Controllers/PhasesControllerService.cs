using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class PhasesControllerService : IPhasesControllerService
	{
		private static readonly IList<PhaseViewModel> PhaseViewModels = new List<PhaseViewModel>();

		public PhasesViewModel GetPhases(SelectorViewModel selectorViewModel)
		{
			if (PhaseViewModels.Count == 0)
			{
				for (var index = 0; index < 10; index++)
				{
					PhaseViewModels.Add(new PhaseViewModel { Name = "phase name #" + index, Identity = "phase_identity_" + index });
				}
			}

			return new PhasesViewModel { Phases = PhaseViewModels.Where(e => e.Name.Contains(selectorViewModel.Search)), SelectorViewModel = selectorViewModel };
		}

		public IMethodResult RemovePhases(string identity)
		{
			PhaseViewModels.Remove(PhaseViewModels.First(e => e.Identity == identity));

			return new MessageMethodResult(true, string.Empty);
		}
	}
}