using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ICartControllerService
	{
		void AddSpotToCart(IElementIdentity spotIdentity);
		void RemoveSpotFromCart(IElementIdentity spotIdentity);
	}
}