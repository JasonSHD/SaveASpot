using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.PhasesAndParcels
{
	public sealed class SpotPhaseContainerQueryable : ISpotPhaseContainerQueryable
	{
		private readonly ISpotQueryable _spotQueryable;
		private readonly IParcelQueryable _parcelQueryable;
		private readonly IPhaseQueryable _phaseQueryable;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public SpotPhaseContainerQueryable(
			ISpotQueryable spotQueryable,
			IParcelQueryable parcelQueryable,
			IPhaseQueryable phaseQueryable,
			IElementIdentityConverter elementIdentityConverter)
		{
			_spotQueryable = spotQueryable;
			_parcelQueryable = parcelQueryable;
			_phaseQueryable = phaseQueryable;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public IEnumerable<SpotPhaseContainer> Find(IEnumerable<IElementIdentity> spotsIdentities)
		{
			var spots = _spotQueryable.Filter(e => e.ByIdentities(spotsIdentities)).ToList();
			var parcels =
				_parcelQueryable.Filter(e => e.ByIdentities(spots.Select(spot => _elementIdentityConverter.ToIdentity(spot.ParcelId))))
												.ToList();
			var phases =
				_phaseQueryable.Filter(
					e => e.ByIdentities(parcels.Select(parcel => _elementIdentityConverter.ToIdentity(parcel.PhaseId)))).ToList();

			foreach (var spot in spots)
			{
				var parcel = parcels.First(e => e.Id == spot.ParcelId);
				var phase = phases.First(e => e.Id == parcel.PhaseId);

				yield return new SpotPhaseContainer { Phase = phase, Spot = spot };
			}
		}
	}
}
