using System.Collections.Generic;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Services.Interfaces
{
	public interface IParcelService
	{
		IEnumerable<Parcel> GetAllParcelsByPhaseId(string phaseId);
	}
}
