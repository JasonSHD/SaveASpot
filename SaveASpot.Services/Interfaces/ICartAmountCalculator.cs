using System.Collections.Generic;
using SaveASpot.Core.Cart;

namespace SaveASpot.Services.Interfaces
{
	public interface ICartAmountCalculator
	{
		decimal CalculateAmount(IEnumerable<SpotElement> spots);
	}
}