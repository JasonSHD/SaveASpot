using System.Collections.Generic;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IPhaseQueryable
	{
		IPhaseFilter All();
		IPhaseFilter ByName(string name);
		IEnumerable<Phase> Find(IPhaseFilter phaseFilter);
	}
}