using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface ISpotRepository
	{
		Spot AddSpot(Spot spot);
		bool Remove(IElementIdentity identity);
		bool Update(Spot spot);
		bool MapSpotToCustomer(Spot spot, IElementIdentity customerIdentity);
		bool MapSpotToSponsor(Spot spot, IElementIdentity sponsorIdentity);
		bool RemoveMap(Spot spot);
		IEnumerable<Spot> GetSpotsByParcelId(string parcelId);
	}
}
