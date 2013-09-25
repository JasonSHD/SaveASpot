using SaveASpot.Core;
using SaveASpot.Core.Geocoding;
using SaveASpot.Services.Interfaces.Geocoding;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IMapControllerService
	{
		MapViewModel GetMapViewModel();
		SquareElementsResult ForSquare(IElementIdentity phaseIdentity, Point topRight, Point bottomLeft);
	}
}