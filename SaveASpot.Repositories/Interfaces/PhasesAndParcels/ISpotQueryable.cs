using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface ISpotQueryable : IElementQueryable<Spot, ISpotFilter>
	{
		ISpotFilter All();
		ISpotFilter ByArea(decimal area);
		ISpotFilter ByParcels(IEnumerable<IElementIdentity> identities);
		ISpotFilter ByIdentity(IElementIdentity identity);
		ISpotFilter ByIdentities(IEnumerable<IElementIdentity> identities);
	}
}