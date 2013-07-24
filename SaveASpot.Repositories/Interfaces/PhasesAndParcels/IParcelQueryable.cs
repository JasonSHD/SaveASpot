using System.Collections.Generic;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IParcelQueryable
	{
		IParcelFilter All();
		IParcelFilter ByPhase(string identity);
		IEnumerable<Parcel> Find(IParcelFilter filter);
	}
}