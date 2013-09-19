using SaveASpot.Core;
using SaveASpot.Core.Geocoding;
using SaveASpot.Services.Interfaces.Geocoding;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ISpotsControllerService
	{
		SpotsViewModel GetSpots(SelectorViewModel selectorViewModel);
		SpotsViewModel ByPhase(IElementIdentity identity);
		IMethodResult Remove(IElementIdentity identity);
		SquareElementsResult ForSquare(IElementIdentity phaseIdentity, Point topRight, Point bottomLeft);
	}
}