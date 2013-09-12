using System.Collections.Generic;
using SaveASpot.Core.Cart;
using SaveASpot.Core.Security;

namespace SaveASpot.ViewModels.Checkout
{
	public sealed class SpotsCheckoutViewModel
	{
		public IEnumerable<SpotElement> Spots { get; set; }
		public decimal Price { get; set; }
	}
}
