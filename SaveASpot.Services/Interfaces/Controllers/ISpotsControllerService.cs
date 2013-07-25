using SaveASpot.Core;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ISpotsControllerService
	{
		SpotsViewModel GetSpots(SelectorViewModel selectorViewModel);
		SpotsViewModel ByParcel(string identity);
		IMethodResult Remove(string identity);
	}
}