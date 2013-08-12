using SaveASpot.Core;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ISpotsControllerService
	{
		SpotsViewModel GetSpots(SelectorViewModel selectorViewModel);
		SpotsViewModel ByPhase(IElementIdentity identity);
		IMethodResult Remove(IElementIdentity identity);
	}
}