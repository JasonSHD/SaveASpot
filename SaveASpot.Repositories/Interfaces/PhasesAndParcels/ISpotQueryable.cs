using System.Collections.Generic;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface ISpotQueryable
	{
		ISpotFilter All();
		ISpotFilter ByArea(decimal area);
		ISpotFilter And(ISpotFilter left, ISpotFilter right);
		ISpotFilter ByParcels(IEnumerable<string> identities);
		IEnumerable<Spot> Find(ISpotFilter spotFilter);
	}
}