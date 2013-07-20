using System.Collections.Generic;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IParcelQueryable
	{
		IParcelFilter All();
		IEnumerable<Parcel> Find(IParcelFilter filter);
	}
}