using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IParcelRepository
	{
		Parcel AddParcel(Parcel parcel);
		bool Remove(IEnumerable<IElementIdentity> identity);
		IEnumerable<Parcel> GetAllParcelsByPhaseId(string phaseId);
	}
}
