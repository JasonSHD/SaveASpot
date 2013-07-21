using System.Collections.Generic;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface ISpotQueryable
	{
		ISpotFilter All();
		ISpotFilter ByArea(decimal area);
		ISpotFilter And(ISpotFilter left, ISpotFilter right);
		IEnumerable<Spot> Find(ISpotFilter spotFilter);
	}
}