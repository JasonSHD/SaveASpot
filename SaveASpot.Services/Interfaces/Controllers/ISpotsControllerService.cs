using SaveASpot.Core;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ISpotsControllerService
	{
		SpotsViewModel GetSpots(SelectorViewModel selectorViewModel);
		IMethodResult Remove(string identity);
	}
}