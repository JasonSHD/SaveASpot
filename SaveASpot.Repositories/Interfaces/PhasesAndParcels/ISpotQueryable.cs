using System.Collections.Generic;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface ISpotQueryable
	{
		ISpotFilter All();
		IEnumerable<Spot> Find(ISpotFilter spotFilter);
	}
}