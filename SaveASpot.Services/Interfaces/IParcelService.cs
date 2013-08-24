using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Services.Interfaces
{
	public interface IParcelService
	{
		IEnumerable<Parcel> GetAllParcelsByPhaseId(string phaseId);
	}
}
