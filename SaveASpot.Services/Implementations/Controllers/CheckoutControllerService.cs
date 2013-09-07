﻿using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.Checkout;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class CheckoutControllerService : ICheckoutControllerService
	{
		private readonly IPhaseQueryable _phaseQueryable;
		private readonly IParcelQueryable _parcelQueryable;
		private readonly ISpotQueryable _spotQueryable;
		private readonly IElementIdentityConverter _elementIdentityConverter;
		private readonly ICurrentCart _currentCart;

		public CheckoutControllerService(IPhaseQueryable phaseQueryable,
			IParcelQueryable parcelQueryable,
			ISpotQueryable spotQueryable,
			IElementIdentityConverter elementIdentityConverter,
			ICurrentCart currentCart)
		{
			_phaseQueryable = phaseQueryable;
			_parcelQueryable = parcelQueryable;
			_spotQueryable = spotQueryable;
			_elementIdentityConverter = elementIdentityConverter;
			_currentCart = currentCart;
		}

		public CheckoutViewModel GetSpots(IElementIdentity phaseIdentity)
		{
			var phase = _phaseQueryable.Filter(e => e.ByIdentity(phaseIdentity)).First();

			if (!phase.SpotPrice.HasValue)
			{
				return new CheckoutViewModel();
			}

			var parcelsForPhase = _parcelQueryable.Filter(e => e.ByPhase(phaseIdentity)).Find().Select(e => _elementIdentityConverter.ToIdentity(e.Id));
			var spotsForParcels = _spotQueryable.Filter(e => e.ByParcels(parcelsForPhase)).Find();

			var elementsInCart =
				spotsForParcels.Where(
					e =>
					_currentCart.Cart.ElementIdentities.Any(
						identity => _elementIdentityConverter.IsEqual(identity, _elementIdentityConverter.ToIdentity(e)))).ToList();

			var price = elementsInCart.Count * phase.SpotPrice.Value;

			return new CheckoutViewModel { Price = price, Spots = elementsInCart.Select(e => Convert(e, phase.SpotPrice.Value)) };
		}

		private SpotViewModel Convert(Spot spot, decimal price)
		{
			return new SpotViewModel
							 {
								 Identity = _elementIdentityConverter.ToIdentity(spot.Id),
								 Price = price
							 };
		}
	}
}