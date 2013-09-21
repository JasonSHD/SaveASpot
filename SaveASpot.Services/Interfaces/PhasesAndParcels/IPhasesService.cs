using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.PhasesAndParcels
{
	public interface IPhasesService
	{
		bool RemovePhase(IElementIdentity phaseIdentity);
	}
}