using SaveASpot.Core;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IPhasesControllerService
	{
		PhasesViewModel GetPhases(SelectorViewModel selectorViewModel);
		IMethodResult RemovePhases(IElementIdentity identity);
		IMethodResult<MessageResult> EditPhase(IElementIdentity identity, PhaseViewModel phaseViewModel);
	}
}