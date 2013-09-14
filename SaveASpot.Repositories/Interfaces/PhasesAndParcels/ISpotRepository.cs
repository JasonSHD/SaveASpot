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
		bool MapSpotToSponsor(Spot spot, IElementIdentity sponsorIdentity);
		bool MapSpotToCustomer(Spot spot, IElementIdentity customerIdentity, decimal? spotPrice);
		IEnumerable<Spot> GetSpotsByParcelId(string parcelId);
	}
}
