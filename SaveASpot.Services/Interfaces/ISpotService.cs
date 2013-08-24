using System.Collections.Generic;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Services.Interfaces
{
	public interface ISpotService
	{
		IEnumerable<Spot> GetSpotsByParcelId(string parcelId);
	}
}
