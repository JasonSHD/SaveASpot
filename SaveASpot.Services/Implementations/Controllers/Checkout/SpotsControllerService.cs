using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers.Checkout;
using SaveASpot.ViewModels.Checkout;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers.Checkout
{
	public sealed class SpotsControllerService : ISpotsControllerService
	{
		private readonly IPhaseQueryable _phaseQueryable;
		private readonly IParcelQueryable _parcelQueryable;
		private readonly ISpotQueryable _spotQueryable;
		private readonly IElementIdentityConverter _elementIdentityConverter;
		private readonly ICurrentCart _currentCart;

		public SpotsControllerService(IPhaseQueryable phaseQueryable,
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

		public SpotsCheckoutViewModel GetSpots()
		{
			var elementsInCart = _currentCart.Cart.ElementIdentities;
			var spotsInCart = _spotQueryable.Filter(e => e.ByIdentities(elementsInCart)).ToList();
			var parcelsIdentitiesInCart = spotsInCart.GroupBy(e => e.ParcelId).Select(e => _elementIdentityConverter.ToIdentity(e.Key));
			var parcelsInCart = _parcelQueryable.Filter(e => e.ByIdentities(parcelsIdentitiesInCart)).ToList();
			var phasesIdentitiesInCart = parcelsInCart.GroupBy(e => e.PhaseId).Select(e => _elementIdentityConverter.ToIdentity(e.Key));
			var phasesInCart = _phaseQueryable.Filter(e => e.ByIdentities(phasesIdentitiesInCart)).ToList();

			var spots = new List<SpotViewModel>();
			decimal price = 0;

			foreach (var spot in spotsInCart)
			{
				var parcel = parcelsInCart.First(e => spot.ParcelId == e.Id);
				var phase = phasesInCart.First(e => parcel.PhaseId == e.Id);

				if (phase.SpotPrice.HasValue)
				{
					spots.Add(Convert(spot, phase.SpotPrice.Value));
					price += phase.SpotPrice.Value;
				}
			}

			return new SpotsCheckoutViewModel { Price = price, Spots = spots };
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
