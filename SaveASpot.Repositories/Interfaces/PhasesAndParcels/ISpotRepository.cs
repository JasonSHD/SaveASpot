﻿using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface ISpotRepository
	{
		Spot AddSpot(Spot spot);
		bool Remove(IElementIdentity identity);
		bool Update(Spot spot);
		bool MapSpotToCustomer(Spot spot, IElementIdentity customerIdentity);
	}
}
