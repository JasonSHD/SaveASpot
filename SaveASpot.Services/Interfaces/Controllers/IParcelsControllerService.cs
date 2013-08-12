using SaveASpot.Core;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IParcelsControllerService
	{
		ParcelsViewModel GetParcels(SelectorViewModel selectorViewModel);
		IMethodResult Remove(IElementIdentity identity);
	}
}