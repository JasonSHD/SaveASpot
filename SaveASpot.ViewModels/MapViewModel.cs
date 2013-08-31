using System.Collections.Generic;

namespace SaveASpot.ViewModels
{
	public sealed class MapViewModel
	{
		public bool ShowCustomerBookingPanel { get; set; }
		public IEnumerable<SponsorViewModel> Sponsors { get; set; }
	}
}