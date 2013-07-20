<<<<<<< HEAD
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

			IEnumerable<PhaseViewModel> phases = PhaseViewModels;
			if (!string.IsNullOrWhiteSpace(selectorViewModel.Search))
			{
				phases = phases.Where(e => e.Name.Contains(selectorViewModel.Search));
			}
			return new PhasesViewModel { Phases = phases, SelectorViewModel = selectorViewModel };
		}

		public IMethodResult RemovePhases(string identity)
		{
			PhaseViewModels.Remove(PhaseViewModels.First(e => e.Identity == identity));

			return new MessageMethodResult(true, string.Empty);
=======
﻿using System.Collections.Generic;
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
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937
		}
	}
}