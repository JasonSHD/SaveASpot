using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces
{
	public interface IPhaseRepository
	{
		Phase AddPhase(Phase phase);
		bool PhaseExists(Phase phase);
	}
}
