using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IPhaseRepository
	{
		Phase AddPhase(Phase phase);
		bool PhaseExists(Phase phase);
		bool RemovePhase(string identity);
	}
}
