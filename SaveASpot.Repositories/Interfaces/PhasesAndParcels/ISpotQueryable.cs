using System.Collections.Generic;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface ISpotQueryable : IElementQueryable<Spot, ISpotFilter>
	{
		ISpotFilter All();
		ISpotFilter ByArea(decimal area);
		ISpotFilter ByParcels(IEnumerable<string> identities);
		ISpotFilter ByIdentity(string identity);
	}
}