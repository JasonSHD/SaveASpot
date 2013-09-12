using System.Collections.Generic;
using SaveASpot.Core.Cart;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers.Checkout;
using SaveASpot.ViewModels.Checkout;

namespace SaveASpot.Services.Implementations.Controllers.Checkout
{
	public sealed class SpotsControllerService : ISpotsControllerService
	{
		private readonly ICurrentCart _currentCart;

		public SpotsControllerService(ICurrentCart currentCart)
		{
			_currentCart = currentCart;
		}

		public SpotsCheckoutViewModel GetSpots()
		{
			var elementsInCart = _currentCart.Cart.SpotElements;

			var spots = new List<SpotElement>();
			decimal price = 0;

			foreach (var spot in elementsInCart)
			{
				price += spot.PhaseElement.SpotPrice;
				spots.Add(spot);
			}

			return new SpotsCheckoutViewModel { Price = price, Spots = spots };
		}
	}
}
