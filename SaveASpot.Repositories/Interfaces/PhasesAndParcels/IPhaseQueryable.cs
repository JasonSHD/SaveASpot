using System.Collections.Generic;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IPhaseQueryable
	{
		IPhaseFilter All();
		IEnumerable<Phase> Find(IPhaseFilter phaseFilter);
	}
}