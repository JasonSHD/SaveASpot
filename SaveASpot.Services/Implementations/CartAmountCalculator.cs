using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core.Cart;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class CartAmountCalculator : ICartAmountCalculator
	{
		public decimal CalculateAmount(IEnumerable<SpotElement> spots)
		{
			return spots.Sum(e => e.PhaseElement.SpotPrice);
		}
	}
}