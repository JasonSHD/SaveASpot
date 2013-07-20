using System.Collections.Generic;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class PhasesViewModel
	{
		public IEnumerable<PhaseViewModel> Phases { get; set; }
		public SelectorViewModel SelectorViewModel { get; set; }
	}
}