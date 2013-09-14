using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IPhaseRepository
	{
		Phase AddPhase(Phase phase);
		bool RemovePhase(IElementIdentity identity);
		bool UpdatePhase(IElementIdentity identity, Phase phase);

	}
}
