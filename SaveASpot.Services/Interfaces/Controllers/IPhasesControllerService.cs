using SaveASpot.Core;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IPhasesControllerService
	{
		PhasesViewModel GetPhases(SelectorViewModel selectorViewModel);
		IMethodResult RemovePhases(string identity);
	}
}