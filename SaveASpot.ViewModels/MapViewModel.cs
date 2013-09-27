using System.Collections.Generic;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.ViewModels
{
	public sealed class MapViewModel
	{
		public bool ShowCustomerBookingPanel { get; set; }
		public string GoogleMapKey { get; set; }
		public IEnumerable<SponsorViewModel> Sponsors { get; set; }
		public IEnumerable<PhaseViewModel> Phases { get; set; }
		public IEnumerable<ParcelViewModel> Parcels { get; set; } 
	}
}