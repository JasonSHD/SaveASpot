using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface ISpotPhaseContainerQueryable
	{
		IEnumerable<SpotPhaseContainer> Find(IEnumerable<IElementIdentity> spotsIdentities);
	}
}