using System.Collections.Generic;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.ViewModels.Checkout
{
	public sealed class SpotsCheckoutViewModel
	{
		public IEnumerable<SpotViewModel> Spots { get; set; }
		public decimal Price { get; set; }
	}
}
