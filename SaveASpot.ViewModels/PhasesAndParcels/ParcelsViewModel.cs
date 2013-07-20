using System.Collections.Generic;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class ParcelsViewModel
	{
		public SelectorViewModel SelectorViewModel { get; set; }
		public IEnumerable<ParcelViewModel> Parcels { get; set; }
	}
}