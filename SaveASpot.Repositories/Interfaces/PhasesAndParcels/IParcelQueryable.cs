using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IParcelQueryable
	{
		IParcelFilter All();
		IParcelFilter ByPhase(IElementIdentity identity);
		IEnumerable<Parcel> Find(IParcelFilter filter);
	}
}