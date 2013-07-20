using System.Collections.Generic;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class SpotsViewModel
	{
		public SelectorViewModel Selector { get; set; }
		public IEnumerable<SpotViewModel> Spots { get; set; }
	}
}